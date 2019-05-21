using System.Collections.Generic;
using System.Linq;
using Amnesia.Domain.Context;
using Amnesia.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Amnesia.Application.Validation
{
    public class DatabaseValidationContext : IValidationContext
    {
        private readonly BlockchainContext context;

        public DatabaseValidationContext(BlockchainContext context)
        {
            this.context = context;
        }

        public Block GetBlockAndContent(byte[] hash)
        {
            return context.Blocks
                .Include(b => b.Content)
                .FirstOrDefault(b => b.Hash == hash);
        }

        public Definition GetDefinition(byte[] hash)
        {
            return context.Definitions.FirstOrDefault(d => d.Hash == hash);
        }

        public IList<Definition> GetDefinitions(byte[] blockHash)
        {
            var block = GetBlockAndContent(blockHash);

            var definitions = block.Content.Definitions;
            return definitions.Select(hash => context.Definitions.Find(hash)).ToList();
        }

        public IList<Definition> GetMutations(byte[] blockHash)
        {
            var block = GetBlockAndContent(blockHash);

            var mutations = block.Content.Mutations;
            return mutations.Select(hash => context.Definitions.Find(hash)).ToList();
        }

        public Data GetData(byte[] definitionHash)
        {
            var definition = context.Definitions
                .Include(d => d.Data)
                .FirstOrDefault(d => d.Hash == definitionHash);

            return definition?.Data;
        }

        public IEnumerable<byte[]> GetBlockGraph(byte[] startHash)
        {
            var hash = startHash;

            do
            {
                yield return hash;
                hash = context.Blocks
                    .Where(b => b.Hash == hash)
                    .Select(b => b.PreviousBlockHash)
                    .SingleOrDefault();
            } while (hash != null);
        }

        public IList<Definition> MissingData { get; set; } = new List<Definition>();
    }
}