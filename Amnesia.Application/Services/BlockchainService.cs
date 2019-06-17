using Amnesia.Application.Validation.Context;
using Amnesia.Domain.Context;

namespace Amnesia.Application.Services
{
    public class BlockchainService
    {
        private readonly BlockchainContext dbContext;

        public BlockchainService(BlockchainContext dbContext)
        {
            this.dbContext = dbContext;
            ValidationContext = new DatabaseValidationContext(this.dbContext);
        }

        public DatabaseValidationContext ValidationContext { get; }

        public void SaveContext(MemoryValidationContext context)
        {
            dbContext.Definitions.AddRange(context.Definitions.Values);
            dbContext.Data.AddRange(context.Data.Values);
            dbContext.Contents.AddRange(context.Contents.Values);
            dbContext.Blocks.AddRange(context.Blocks.Values);
            dbContext.SaveChanges();
        }
    }
}