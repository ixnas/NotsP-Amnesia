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

        private void CheckBlock()
        {
            var miner = new Miner(10);
        }
        
        public void ReceiveDefinition(Definition definition)
        {
            throw new NotImplementedException();
        }
    }
}