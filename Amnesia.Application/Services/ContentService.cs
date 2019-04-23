using System.Threading.Tasks;
using Amnesia.Domain.Context;
using Amnesia.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Amnesia.Application.Services
{
    public class ContentService
    {
        private readonly BlockchainContext blockchainContext;

        public ContentService(BlockchainContext blockchainContext)
        {
            this.blockchainContext = blockchainContext;
        }

        public Task<Content> GetContent(byte[] hash)
        {
            return blockchainContext.Contents
                .Include(c => c.Definitions)
                .Include(c => c.Mutations)
                .SingleOrDefaultAsync(c => c.Hash == hash);
        }

        public async Task<Content> AddContent(Content content)
        {
            var result = await blockchainContext.Contents.AddAsync(content);
            return result.Entity;
        }
    }
}