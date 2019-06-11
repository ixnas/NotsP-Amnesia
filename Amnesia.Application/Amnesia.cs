﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amnesia.Application.Mining;
using Amnesia.Application.Peers;
using Amnesia.Application.Services;
using Amnesia.Application.Validation;
using Amnesia.Application.Validation.Context;
using Amnesia.Application.Validation.Result;
using Amnesia.Domain.Entity;
using Amnesia.Domain.Model;

namespace Amnesia.Application
{
    public class Amnesia
    {
        private readonly PeerManager peerManager;
        private readonly StateService stateService;
        private readonly BlockchainService blockchain;

        private const int Difficulty = 20;

        public Amnesia(PeerManager peerManager, StateService stateService, BlockchainService blockchain)
        {
            this.peerManager = peerManager;
            this.stateService = stateService;
            this.blockchain = blockchain;
        }

        public async Task ReceiveBlock(byte[] blockHash, string sendingPeer)
        {
            var peer = peerManager.GetPeer(sendingPeer);
            var memoryContext = new MemoryValidationContext();
            
            var peerGraph = peerManager.GetBlocks(peer).Result.Value.ToList();
            var currentGraph = blockchain.ValidationContext.GetBlockGraph(stateService.State.CurrentBlockHash).ToList();
            
            if (peerGraph.Count <= currentGraph.Count)
            {
                return;
            }
            
            FillMemoryContext(peerGraph, currentGraph, peer, memoryContext);

            var combinedValidationContext = new CombinedValidationContext
            {
                memoryContext,
                blockchain.ValidationContext
            };
            var blockValidator = new BlockValidator(combinedValidationContext, Difficulty);
            var result = blockValidator.ValidateBlock(peerGraph.FirstOrDefault());
            if (result is BlockSuccessResult)
            {
                //TODO: ExecuteMutation
                blockchain.SaveContext(memoryContext);
                stateService.ChangeState(peerGraph.FirstOrDefault());
            }
        }

        //TODO: Fetch missing data
        private void FillMemoryContext(IEnumerable<byte[]> peerGraph, IEnumerable<byte[]> currentGraph, Peer peer, 
            MemoryValidationContext memoryContext)
        {
            var missingBlocks = peerGraph.Except(currentGraph).ToList();
             
            foreach (var hash in missingBlocks)
            {
                var block = peerManager.GetBlock(peer, Hash.ByteArrayToString(hash)).Result.Value.ToBlock();
                var content = peerManager.GetContent(peer, Hash.ByteArrayToString(block.ContentHash)).Result.Value.ToContent();

                foreach (var definitionHash in content.Definitions.Concat(content.Mutations))
                {
                    var definition = peerManager.GetDefinition(peer, Hash.ByteArrayToString(definitionHash)).Result.Value.ToDefinition();
                    var data = peerManager.GetData(peer, Hash.ByteArrayToString(definition.DataHash)).Result.Value.ToData();
                    memoryContext.AddData(data);
                    memoryContext.AddDefinition(definition);
                }
                memoryContext.AddContent(content);
                memoryContext.AddBlock(block);
            }
        }
        
        public async Task ReceiveDefinition(Definition definition, Data data)
        {           
            var previousBlock = stateService.State.CurrentBlock;
            var newContent = new Content();

            if (definition.IsMutation)
            {
                newContent.Mutations.Add(definition.Hash);
            }
            else
            {
                newContent.Definitions.Add(definition.Hash);
            }

            newContent.Hash = newContent.HashObject();

            var blockToMine = new Block
            {
                PreviousBlockHash = previousBlock.Hash,
                Nonce = 0,
                ContentHash = newContent.Hash,
                Content = newContent,
                PreviousBlock = previousBlock
            };

            var miner = new Miner(Difficulty);
            await miner.Start(blockToMine);

            var context = new MemoryValidationContext();
            context.AddBlock(blockToMine);
            context.AddContent(blockToMine.Content);
            context.AddDefinition(definition);
            context.AddData(data);

            blockchain.SaveContext(context);
            stateService.ChangeState(blockToMine.Hash);
            
            foreach (var peerKey in peerManager.GetPeers())
            {
                var peerToSend = peerManager.GetPeer(peerKey);
                peerManager.PostBlock(stateService.State.PeerId, peerToSend, Hash.ByteArrayToString(blockToMine.Hash));
            }
        }
    }
}

//TODO: EXECUTE MUTATION            
//if(mutation == valid && newChain > currentChain)
//           var mutation = new Definition
//           {
//               PreviousDefinitionHash = Hash.StringToByteArray("d9cb74f22c33625e37be48e5ef5ce9dc18d9e605338c2dc83b66c713d3d7ba41"),
//               IsMutable = false,
//               IsMutation = true
//           };
//           var peer = peerManager.GetPeer("peer1");
//           var previous = await peerManager.GetDefinition(peer, Hash.ByteArrayToString(mutation.PreviousDefinitionHash));         
//           dataService.RemoveDataThroughMutation(Hash.StringToByteArray(previous.Value.DataHash));