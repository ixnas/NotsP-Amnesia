using System;
using System.Collections.Generic;
using System.Linq;
using Amnesia.Domain.Entity;

namespace Amnesia.Application.Peers
{
    public class PeerManager
    {
        private readonly IDictionary<string, Peer> peers;

        public PeerManager(IEnumerable<Peer> peers)
        {
            this.peers = new Dictionary<string, Peer>(
                peers.Select(p => new KeyValuePair<string, Peer>(p.Key, p)));
        }

        public Peer GetPeer(string key)
        {
            return peers[key];
        }

        public Block GetBlock(string key, string hash)
        {
            throw new NotImplementedException();
        }

        public Definition GetDefinition(string key, string hash)
        {
            throw new NotImplementedException();
        }
        
        public List<Definition> GetDefinitions(string key, int limit)
        {
            throw new NotImplementedException();
        }

        public Content GetContent(string key, string hash)
        {
            throw new NotImplementedException();
        }
    }
}