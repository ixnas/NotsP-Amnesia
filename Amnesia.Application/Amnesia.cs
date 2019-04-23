using System;
using Amnesia.Application.Peers;
using Amnesia.Domain.Entity;

namespace Amnesia.Application
{
    public class Amnesia
    {
        private readonly PeerManager peerManager;

        public Amnesia(PeerManager peerManager)
        {
            this.peerManager = peerManager;
        }

        public Block CurrentBlock { get; }
    }
}