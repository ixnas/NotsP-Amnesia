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

        public Block GetBlock(byte[] hash)
        {   
            return context.Blocks.FirstOrDefault(b => b.Hash == hash);
        }

        public async Task<List<Block>> GetBlocks(int depth)
        {
            List<Block> blocks = new List<Block>();
            var block = await context.Blocks.FirstOrDefaultAsync();
            blocks.Add(block);
            
            for (int i = 0; i < depth - 1; i++)
            {
                var previousBlock = await context.Blocks.SingleOrDefaultAsync(
                    b => b.Hash == blocks.Last().PreviousBlockHash);
                blocks.Add(previousBlock);
            }

            return blocks;
        }
        
        public List<Block> GetBlocks()
        {
            return context.Blocks.ToList();
        }

        public async Task<Block> AddBlock(Block block)
        {
            var result = await context.Blocks.AddAsync(block);
            return result.Entity;
        }
    }
}