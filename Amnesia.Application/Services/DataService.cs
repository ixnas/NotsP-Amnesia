using System.Threading.Tasks;
using Amnesia.Domain.Context;
using Amnesia.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Amnesia.Application.Services
{
    public class DataService
    {
        private readonly BlockchainContext context;

        public DataService(BlockchainContext context)
        {
            this.context = context;
        }

        public Task<Data> GetData(byte[] hash)
        {
            return context.Data.FirstOrDefaultAsync(d => d.Hash == hash);
        }
    }
}