using System;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
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
           
            Console.WriteLine("Ik ben hier");
            
            var peer = peerManager.GetPeer(sendingPeer);

            Console.WriteLine(peer.Key);
            Console.WriteLine(peer.Url);
            
            var blockData = await peerManager.GetBlock(peer, Hash.ByteArrayToString(blockHash));
            
            Console.WriteLine(blockData.Value.Hash);
            
            //Get alle gegevens
            //Get specific block from hash 
        }

        public void ReceiveDefinition(Definition definition)
        {
            throw new NotImplementedException();
        }
    }
}
//    public class Request()
//    {
//        private HttpClient httpClient;
//
//        public Request(HttpClient httpClient)
//        {
//            this.httpClient = httpClient;
//        }
//
//        public async Task<BlockViewModel> GetBlockData(string url, string blockHash)
//        {
//            string request = url + "/blocks/" + blockHash;
//            var response = await httpClient.GetAsync(request);
//            var content = await response.Content.ReadAsStringAsync();
//            return JsonConvert.DeserializeObject<BlockViewModel>(content);
//        }
//    }
//}