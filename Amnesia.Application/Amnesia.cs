using System;
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
<<<<<<< HEAD
        private readonly ContentService contentService;
        private readonly BlockService blockService;
        private readonly int difficulty = 20;
=======
        private readonly DataService dataService;
>>>>>>> 550f320b436548bd94b6a7c81a08aedc5c30e94f

        public Amnesia(PeerManager peerManager, StateService stateService, DataService dataService)
        {
            this.peerManager = peerManager;
            this.stateService = stateService;
<<<<<<< HEAD
            this.contentService = null; // needs to be configured
            this.blockService = null; // needs to be configured
=======
            this.dataService = dataService;
>>>>>>> 550f320b436548bd94b6a7c81a08aedc5c30e94f
        }

        public Block CurrentBlock => stateService.State.CurrentBlock;

        public async Task ReceiveBlock(byte[] blockHash, string sendingPeer)
        {
            Console.WriteLine("Received a block.");
            var peer = peerManager.GetPeer(sendingPeer);
            
            var blockData = await peerManager.GetBlock(peer, Hash.ByteArrayToString(blockHash));
            var contentData = await peerManager.GetContent(peer, blockData.Value.Content);
            Console.WriteLine(blockData.Value.Hash);
            Console.WriteLine(contentData.Value.Hash);
            Console.WriteLine(contentData.Value.Definitions.First());
            
            //CheckBlock(){}
            //Get alle gegevens
            //Get specific block from hash 
        }

        //TODO: Write implementation for checking block (Consensus).
        private void CheckBlock()
        {
            throw new NotImplementedException();
            var miner = new Miner(10);
        }
        
        public async Task ReceiveDefinition(Definition definition)
        {
            var content = await contentService.GetContent(definition.Hash); //QUESTION: Ik haal content op met definition hash? 

            content.Definitions.Add(definition.Hash);
            var blocks = blockService.GetBlocks();
            var lastBlock = blocks.Last();

            var payload = new Block
            {
                Nonce = 0,
                PreviousBlockHash = lastBlock.Hash,
                PreviousBlock = lastBlock,
                Content = content,
                ContentHash = content.Hash
            };

            var miner = new Miner(difficulty);

            _ = miner.Start(payload);

            miner.Mined += b =>
            {
                //TODO: send verified block to connected peers
            };            

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