using System;
using System.Linq;
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

        public void RemoveDataThroughMutation(byte[] hash)
        {
            var data = context.Data.SingleOrDefault(d => d.Hash == hash);
            context.Data.Remove(data ?? throw new ArgumentNullException());
            context.SaveChanges();
        }
    }
}