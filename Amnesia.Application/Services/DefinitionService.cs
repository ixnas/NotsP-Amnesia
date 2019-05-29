﻿using System.Threading.Tasks;
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

        public Task<Definition> GetDefinition(byte[] hash)
        {
            return blockchainContext.Definitions
                .Include(d => d.Data)
                .SingleOrDefaultAsync(d => d.Hash == hash);
        }
        
         /// <summary>
         /// Returns te last definition that was added to the chain with a public key.
         /// </summary>
         /// <returns></returns>
        public Task<Definition> GetLastDefinition(string key)
        {
            return blockchainContext.Definitions
                .Include(d => d.Data)
                .FirstOrDefaultAsync(d => d.Key == key);
        }
        /// <summary>
        /// Returns te last definition that was added to the chain.
        /// </summary>
        /// <returns></returns>
        public Task<Definition> GetLastDefinition()
        {
            return blockchainContext.Definitions
                .Include(d => d.Data)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="definition"></param>
        /// <returns></returns>

        public async Task<Definition> AddDefinition(Definition definition)
        {
            var result = await blockchainContext
                .Definitions
                .AddAsync(definition);

            return result.Entity;
        }
    }
}