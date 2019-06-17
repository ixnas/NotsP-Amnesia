using System;
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

            var graph = await peerManager.GetBlocks(peer);
            var peerGraph = graph.Value.ToList();
            var currentGraph = blockchain.ValidationContext.GetBlockGraph(stateService.State.CurrentBlockHash).ToList();
            
            if (peerGraph.Count <= currentGraph.Count)
            {
                return;
            }
            
            await FillMemoryContext(peerGraph, currentGraph, peer, memoryContext);

            var combinedValidationContext = new CombinedValidationContext
            {
                memoryContext,
                blockchain.ValidationContext
            };
            var blockValidator = new BlockValidator(combinedValidationContext, Difficulty);
            var result = blockValidator.ValidateBlock(peerGraph.FirstOrDefault());
            if (!(result is BlockSuccessResult))
            {
                Console.WriteLine(result.Message);
                Console.WriteLine("Not Valid");
                return;
            }

            Console.WriteLine("Valid");
            blockchain.SaveContext(memoryContext);
            stateService.ChangeState(peerGraph.FirstOrDefault());
            ExecuteMutations(blockchain.ValidationContext, memoryContext.Blocks.Keys);
        }

        //TODO: Fetch missing data
        private async Task FillMemoryContext(IEnumerable<byte[]> peerGraph, IEnumerable<byte[]> currentGraph, Peer peer, 
            MemoryValidationContext memoryContext)
        {
            var missingBlocks = peerGraph.Where(p => !currentGraph.Any(l => p.SequenceEqual(l)));
             
            foreach (var hash in missingBlocks)
            {
                var blockVm = await peerManager.GetBlock(peer, Hash.ByteArrayToString(hash));
                var block = blockVm.Value.ToBlock();
                var contentVm = await peerManager.GetContent(peer, Hash.ByteArrayToString(hash));
                var content = contentVm.Value.ToContent();
                
                foreach (var definitionHash in content.Definitions.Concat(content.Mutations))
                {
                    var definitionVm = await peerManager.GetDefinition(peer, Hash.ByteArrayToString(definitionHash));
                    var definition = definitionVm.Value.ToDefinition();
                    var dataVm = await peerManager.GetData(peer, Hash.ByteArrayToString(definitionHash));
                    if (dataVm.HasValue)
                    {
                        var data = dataVm.Value.ToData();
                        memoryContext.AddData(data);
                    }
                    memoryContext.AddDefinition(definition);
                }
                memoryContext.AddContent(content);
                memoryContext.AddBlock(block);
            }
        }
        
        public async Task ReceiveDefinition(Definition definition)
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
            context.AddData(definition.Data);

            blockchain.SaveContext(context);
            stateService.ChangeState(blockToMine.Hash);
            ExecuteMutations(blockchain.ValidationContext, new[] {blockToMine.Hash});
            
            foreach (var peerKey in peerManager.GetPeers())
            {
                var peerToSend = peerManager.GetPeer(peerKey);
                peerManager.PostBlock(stateService.State.PeerId, peerToSend, Hash.ByteArrayToString(blockToMine.Hash));
            }
        }

        public void ExecuteMutations(DatabaseValidationContext database, IEnumerable<byte[]> blockHashes)
        {
            var definitions = blockHashes
                .SelectMany(database.GetMutations)
                .ToList();
            
            var dataHashes = definitions
                .Select(m => database.GetData(m.Hash)).ToList()
                .Select(d => d.ParseMutationHash()).ToList()
                .Select(definitionHash => database.GetDefinition(definitionHash).DataHash).ToList();

            database.DeleteData(dataHashes);
        }

        private IEnumerable<Definition> FindMutations(IValidationContext context, IEnumerable<byte[]> blockHashes)
        {
            return blockHashes.SelectMany(context.GetMutations);
        }
    }
}
