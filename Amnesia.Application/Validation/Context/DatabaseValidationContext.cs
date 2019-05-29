using System.Collections.Generic;
using System.Linq;
using Amnesia.Domain.Context;
using Amnesia.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Amnesia.Application.Validation.Context
{
    public class DatabaseValidationContext : IValidationContext
    {
        private readonly BlockchainContext context;

        public DatabaseValidationContext(BlockchainContext context)
        {
            this.context = context;
        }

        public bool HasBlock(byte[] hash)
        {
            return context.Blocks
                .Any(b => b.Hash == hash);
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

        public bool HasDefinition(byte[] hash)
        {
            return context.Definitions
                .Any(d => d.Hash == hash);
        }

        public IList<Definition> GetDefinitions(byte[] blockHash)
        {
            var block = GetBlockAndContent(blockHash);

            var definitions = block?.Content.Definitions;
            return definitions?.Select(hash => context.Definitions.Find(hash)).ToList();
        }

        public IList<Definition> GetMutations(byte[] blockHash)
        {
            var block = GetBlockAndContent(blockHash);

            var mutations = block?.Content.Mutations;
            return mutations?.Select(hash => context.Definitions.Find(hash)).ToList();
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
                hash = GetPreviousBlock(hash);
            } while (hash != null);
        }

        public byte[] GetPreviousBlock(byte[] hash)
        {
            return context.Blocks
                .Where(b => b.Hash == hash)
                .Select(b => b.PreviousBlockHash)
                .SingleOrDefault();
        }

        public IList<Definition> MissingData { get; set; } = new List<Definition>();

        public bool ShouldAssumeValid(byte[] blockHash)
        {
            return HasBlock(blockHash);
        }

        public IEnumerable<byte[]> GetDefinitionsByKey(string key, byte[] startBlock)
        {
            var graph = GetBlockGraph(startBlock);

            foreach (var blockHash in graph)
            {
                var content = context.Blocks
                    .Where(b => b.Hash == blockHash)
                    .Select(b => b.Content)
                    .Single();

                // Loop definitions backwards
                var definitionsInBlock = content.Mutations.Reverse().Concat(content.Definitions.Reverse()).ToList();

                var definitionsFromKey = context.Definitions
                    .Where(d => definitionsInBlock.Contains(d.Hash) && 
                                d.Key == key)
                    .Select(d => d.Hash)
                    .ToList();

                // definitionsFromKey may not be in order
                var orderedDefinitions = definitionsInBlock
                    .Select(h => definitionsFromKey.FirstOrDefault(d => d == h))
                    .Where(h => h != null);

                foreach (var definition in orderedDefinitions)
                {
                    yield return definition;
                }
            }
        }
    }
}