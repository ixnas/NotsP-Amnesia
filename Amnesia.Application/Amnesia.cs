using System;
using System.Linq;
using System.Threading.Tasks;
using Amnesia.Application.Mining;
using Amnesia.Application.Peers;
using Amnesia.Application.Services;
using Amnesia.Application.Validation.Context;
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

            foreach (var peerKey in peerManager.GetPeers())
            {
                if (peerKey.Equals(state.PeerId)) continue;
                var peerToSend = peerManager.GetPeer(peerKey);
                peerManager.PostBlock(peer, peerToSend, Hash.ByteArrayToString(blockToMine.Hash));
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
//           
//           dataService.RemoveDataThroughMutation(Hash.StringToByteArray(previous.Value.DataHash));