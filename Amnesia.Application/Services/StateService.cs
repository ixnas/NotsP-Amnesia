using System;
using System.Linq;
using Amnesia.Domain.Context;
using Amnesia.Domain.Entity;

namespace Amnesia.Application.Services
{
    public class StateService
    {
        private readonly BlockchainContext blockchainContext;

        public StateService(BlockchainContext blockchainContext)
        {
            this.blockchainContext = blockchainContext;
            lazyState = new Lazy<State>(() =>
            {
                Ensure();
                return blockchainContext.State.Single();
            });
        }

        private readonly Lazy<State> lazyState;

        public State State => lazyState.Value;

        public void SaveChanges()
        {
            blockchainContext.SaveChanges();
        }

        private void Ensure()
        {
            var count = blockchainContext.State.Count();

            if (count == 0)
            {
                blockchainContext.State.Add(new State());
                blockchainContext.SaveChanges();
            }

            else if (count > 1)
            {
                var first = blockchainContext.State.First();
                blockchainContext.State.RemoveRange(blockchainContext.State);
                blockchainContext.State.Add(first);
                blockchainContext.SaveChanges();
            }
        }
    }
}