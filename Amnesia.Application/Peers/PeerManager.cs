using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Amnesia.Application.Helper;
using Amnesia.Domain.Entity;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
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

        public Peer? GetPeer(string key)
        {
            return configuration.Peers.ContainsKey(key) 
                ? new Peer(key, configuration.Peers[key]) 
                : null;
        }

        public Task<Maybe<Block>> GetBlock(Peer peer, string hash)
        {
            var url = peer.Url + string.Format(configuration.Api.Blocks, hash);
            // TODO viewmodel
            return GetData<Block>(url);
        }

        public Task<Maybe<Definition>> GetDefinition(Peer peer, string hash)
        {
            var url = peer.Url + string.Format(configuration.Api.Definitions, hash);
            // TODO viewmodel
            return GetData<Definition>(url);
        }

        public Task<Maybe<Content>> GetContent(Peer peer, string hash)
        {
            var url = peer.Url + string.Format(configuration.Api.Contents, hash);
            // TODO viewmodel
            return GetData<Content>(url);
        }

        private static async Task<Maybe<T>> GetData<T>(string url)
        {
            var client = new HttpClient();
            var result = await client.GetAsync(url);

            return result.IsSuccessStatusCode 
                ? new Maybe<T>(JsonConvert.DeserializeObject<T>(result.Content.ToString()))
                : new Maybe<T>();
        }
    }
}