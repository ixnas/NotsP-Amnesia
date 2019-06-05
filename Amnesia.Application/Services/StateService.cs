using System.Linq;
using Amnesia.Application.Peers;
using Amnesia.Domain.Context;
using Amnesia.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Amnesia.Application.Services
{
    public class StateService
    {
        private readonly BlockchainContext dbContext;
        private readonly PeerConfiguration peerConfiguration;

        public StateService(BlockchainContext dbContext, IOptions<PeerConfiguration> peerConfiguration)
        {
            this.dbContext = dbContext;
            this.peerConfiguration = peerConfiguration.Value;
        }

        public State State => FetchAndEnsureState();

        public void SaveChanges()
        {
            dbContext.SaveChanges();
        }

        private State FetchState()
        {
            return dbContext.State
                .Include(s => s.CurrentBlock)
                .SingleOrDefault(s => s.PeerId == peerConfiguration.NetworkId);
        }

        private State FetchAndEnsureState()
        {
            var state = FetchState();

            if (state == null)
            {
                state = new State
                {
                    PeerId = peerConfiguration.NetworkId,
                    CurrentBlockHash = null
                };

                dbContext.State.Add(state);
                dbContext.SaveChanges();
            }

            return state;
        }

        public void ChangeState(byte[] blockHash)
        {
            State.CurrentBlockHash = blockHash;
            SaveChanges();
        }
    }
}