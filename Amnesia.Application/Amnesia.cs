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
        private readonly ContentService contentService;
        private readonly BlockService blockService;
        private readonly int difficulty = 20;

        public Amnesia(PeerManager peerManager, StateService stateService)
        {
            this.peerManager = peerManager;
            this.stateService = stateService;
            this.contentService = null; // needs to be configured
            this.blockService = null; // needs to be configured
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
        }
    }
}