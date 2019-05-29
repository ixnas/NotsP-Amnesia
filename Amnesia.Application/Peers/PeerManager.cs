using System;
using System.Collections.Generic;
using System.Net.Http;
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

        public Task<Maybe<BlockViewModel>> GetBlock(Peer peer, string hash)
        {
            var url = (peer.Url + configuration.Api.Blocks + hash).Trim();
            return GetData<BlockViewModel>(url);
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