using System.Threading.Tasks;
using Amnesia.Domain.Context;
using Amnesia.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Amnesia.Application.Services
{
    public class DefinitionService
    {
        private readonly BlockchainContext blockchainContext;

        public DefinitionService(BlockchainContext blockchainContext)
        {
            this.blockchainContext = blockchainContext;
        }

        public Task<Definition> GetDefinition(byte[] hash, bool includeData = false)
        {
            if (includeData)
            {
                return blockchainContext.Definitions
                    .Include(d => d.Data)
                    .SingleOrDefaultAsync(d => d.Hash == hash);
            }
            return blockchainContext.Definitions
                .SingleOrDefaultAsync(d => d.Hash == hash);
        }

        /*
         * Returns te last definition that was added to the chain.
         * This function can get used for finding out the latest definition hash if you wanna calculate the PreviousDefinitionHash.
         * 
         * @params: null
         * @returns: Task<Definition>
         */
        public Task<Definition> GetLastDefinition()
        {
            return blockchainContext.Definitions.FirstOrDefaultAsync();
        }

        public async Task<Definition> AddDefinition(Definition definition)
        {
            var result = await blockchainContext.Definitions.AddAsync(definition);
            return result.Entity;
        }
    }
}