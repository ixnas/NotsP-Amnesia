using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Amnesia.Application.Mining;
using Amnesia.Application.Peers;
using Amnesia.Application.Services;
using Amnesia.Domain.Entity;
using Amnesia.Domain.Model;
using Amnesia.Domain.ViewModels;
using Newtonsoft.Json;

namespace Amnesia.Application
{
    public class Amnesia
    {
        private readonly PeerManager peerManager;
        private readonly StateService stateService;
        private readonly BlockService blockService;
        
        private readonly int difficulty = 20;

        public Amnesia(PeerManager peerManager, StateService stateService, BlockService blockService)
        {
            this.peerManager = peerManager;
            this.stateService = stateService;
            this.blockService = blockService;
        }

        public Block CurrentBlock => stateService.State.CurrentBlock;

        public async Task ReceiveBlock(byte[] blockHash, string sendingPeer)
        {
            Console.WriteLine("Received a block.");
            var peer = peerManager.GetPeer(sendingPeer);
            Console.WriteLine(Hash.ByteArrayToString(blockHash));
            var blockData = await peerManager.GetBlock(peer, Hash.ByteArrayToString(blockHash));
            var contentData = await peerManager.GetContent(peer, blockData.Value.Content);
            Console.WriteLine(blockData.Value.Hash);
            Console.WriteLine(contentData.Value.Hash);
            Console.WriteLine(contentData.Value.Definitions.First());
        }

        //TODO: Write implementation for checking block (Consensus).
        private void CheckBlock()
        {
            throw new NotImplementedException();
            var miner = new Miner(10);
        }
        
        public async Task ReceiveDefinition(Definition definition)
        {
            var state = stateService.State;
            var peer = peerManager.GetPeer(state.PeerId);
            
            var previousBlock = state.CurrentBlock;
            var newContent = new Content
            {
                Mutations = new List<byte[]>(),
                Definitions = new List<byte[]>()
            };
            
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

            var miner = new Miner(20);
            miner.Mined += newBlock =>
            {
                blockService.SaveBlock(newBlock);
                stateService.ChangeState(peer.Key, newBlock);
                
                foreach (var peerKey in peerManager.GetPeers())
                {
                    if (peerKey.Equals(state.PeerId)) continue;
                    var peerToSend = peerManager.GetPeer(peerKey);
                    peerManager.PostBlock(peer, peerToSend, Hash.ByteArrayToString(newBlock.Hash));
                }
            };          
            await miner.Start(blockToMine);
            
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
//           
//           dataService.RemoveDataThroughMutation(Hash.StringToByteArray(previous.Value.DataHash));
        }
    }
}