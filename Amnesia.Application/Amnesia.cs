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

        public Amnesia(PeerManager peerManager, StateService stateService)
        {
            this.peerManager = peerManager;
            this.stateService = stateService;
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
        
        public async void ReceiveDefinition(Definition definition)
        {
            var contentService = new ContentService(null); //TODO: Needs to be configured
            var content = await contentService.GetContent(definition.Hash);
            int difficulty = 20;

            content.Definitions.Add(new byte[0]); //TODO: add definition to list with content? with a byte[]?

            var previousBlock = content.Definitions.Last(); //TODO: get previous block hash 

            var payload = new Block
            {
                Nonce = 0,
                PreviousBlockHash = new byte[0],
                PreviousBlock = new Block(),
                Content = content,
                ContentHash = content.Hash
            };

            var miner = new Miner(difficulty);
            //TODO: subscribe to event van miner

            await miner.Start(payload);

            //TODO: get verified block from subscribed event

            //TODO: send verified block to connected 
        }
    }
}