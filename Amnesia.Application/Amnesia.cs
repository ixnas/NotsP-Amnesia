using System;
using Amnesia.Application.Peers;
using Amnesia.Application.Services;
using Amnesia.Domain.Entity;

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

        public void ReceiveBlock(byte[] blockHash, string sendingPeer)
        {
            throw new NotImplementedException();
        }

        public void ReceiveDefinition(Definition definition)
        {
            throw new NotImplementedException();
        }
    }
}