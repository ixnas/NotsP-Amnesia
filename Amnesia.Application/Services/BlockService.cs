using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amnesia.Domain.Context;
using Amnesia.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Amnesia.Application.Services
{
    public class BlockService
    {
        private readonly BlockchainContext context;

        public BlockService(BlockchainContext context)
        {
            this.context = context;
        }

        public Task<Block> GetBlock(byte[] hash)
        {
            return context.Blocks.SingleOrDefaultAsync(b => b.Hash == hash);
        }

        public Task<IQueryable<Block>> GetBlocks(int depth)
        {
            return Task.FromResult(context.Blocks.Take(depth));
        }

        public async Task<Block> AddBlock(Block block)
        {
            var result = await context.Blocks.AddAsync(block);
            return result.Entity;
        }
    }
}