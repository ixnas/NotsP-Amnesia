using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Amnesia.Application.Helper;
using Amnesia.Domain.ViewModels;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Amnesia.Application.Peers
{
    public class PeerManager
    {
        private readonly PeerConfiguration configuration;

        public PeerManager(IOptions<PeerConfiguration> configuration)
        {
            this.configuration = configuration.Value;
        }

        public Peer GetPeer(string key)
        {
            return configuration.Peers.ContainsKey(key) 
                ? new Peer(configuration.Peers[key], key ) 
                : null;
        }

        public List<string> GetPeers()
        {
            return configuration.Peers.Keys.ToList();
        }

        public Task<Maybe<BlockViewModel>> GetBlock(Peer peer, string hash)
        {
            var url = (peer.Url + configuration.Api.Blocks + hash).Trim();
            return GetData<BlockViewModel>(url);
        }
        
        public Task<Maybe<List<BlockViewModel>>> GetBlocks(Peer peer)
        {
            var url = (peer.Url + configuration.Api.Blocks).Trim();
            return GetData<List<BlockViewModel>>(url);
        }

        public Task<Maybe<DefinitionViewModel>> GetDefinition(Peer peer, string hash)
        {
            var url = (peer.Url + configuration.Api.Definitions + hash).Trim();
            return GetData<DefinitionViewModel>(url);
        }
        
        public Task<Maybe<IEnumerable<string>>> GetDefinitions(Peer peer, string key, int limit)
        {
            var url = peer.Url + string.Format(configuration.Api.Keys, key, limit);
            return GetData<IEnumerable<string>>(url);
        }

        public Task<Maybe<ContentViewModel>> GetContent(Peer peer, string hash)
        {
            var url = (peer.Url + configuration.Api.Contents + hash).Trim();
            return GetData<ContentViewModel>(url);
        }

        public Task PostBlock(Peer peer, Peer peerToSend, string hash)
        {
            var client = new HttpClient();
            var url = peerToSend.Url + string.Format(configuration.Api.SendBlock, peer.Key);
            var payload = JsonConvert.SerializeObject(hash);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            Console.WriteLine(url); 
            return client.PostAsync(url, content);
        }

        private static async Task<Maybe<T>> GetData<T>(string url)
        {
            var client = new HttpClient();
            var result = await client.GetAsync(url);

            return result.IsSuccessStatusCode 
                ? new Maybe<T>(JsonConvert.DeserializeObject<T>(await result.Content.ReadAsStringAsync()))
                : new Maybe<T>();
        }       
    }
}