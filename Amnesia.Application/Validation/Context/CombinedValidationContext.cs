using System.Collections.Generic;
using System.Linq;
using Amnesia.Domain.Entity;

namespace Amnesia.Application.Validation.Context
{
    public class CombinedValidationContext : List<IValidationContext>, IValidationContext
    {

        public bool HasBlock(byte[] hash)
        {
            return this.Any(c => c.HasBlock(hash));
        }

        public Block GetBlockAndContent(byte[] hash)
        {
            return this
                .Select(c => c.GetBlockAndContent(hash))
                .FirstOrDefault(b => b != null);
        }

        public Definition GetDefinition(byte[] hash)
        {
            return this
                .Select(c => c.GetDefinition(hash))
                .FirstOrDefault(d => d != null);
        }

        public bool HasDefinition(byte[] hash)
        {
            return this.Any(c => c.HasDefinition(hash));
        }

        public IList<Definition> GetDefinitions(byte[] blockHash)
        {
            return this
                .Select(c => c.GetDefinitions(blockHash))
                .FirstOrDefault(d => d != null);
        }

        public IList<Definition> GetMutations(byte[] blockHash)
        {
            return this
                .Select(c => c.GetMutations(blockHash))
                .FirstOrDefault(d => d != null);
        }

        public Data GetData(byte[] definitionHash)
        {
            return this
                .Select(c => c.GetData(definitionHash))
                .FirstOrDefault(d => d != null);
        }

        public IEnumerable<byte[]> GetBlockGraph(byte[] startHash)
        {
            var hash = startHash;

            foreach (var context in this)
            {
                if (!context.HasBlock(hash))
                {
                    continue;
                }

                foreach (var h in context.GetBlockGraph(hash))
                {

                    if (h.SequenceEqual(hash))
                    {
                        continue;
                    }
                    yield return h;
                    hash = h;
                }
            }
        }

        public byte[] GetPreviousBlock(byte[] hash)
        {
            return this
                .Select(c => c.GetPreviousBlock(hash))
                .FirstOrDefault(h => h != null);
        }

        public bool ShouldAssumeValid(byte[] blockHash)
        {
            return this.Any(c => c.ShouldAssumeValid(blockHash));
        }

        public IEnumerable<byte[]> GetDefinitionsByKey(string key, byte[] startBlock)
        {
            var graph = GetBlockGraph(startBlock);

            foreach (var blockHash in graph)
            {
                var content = GetBlockAndContent(blockHash).Content;

                // Loop definitions backwards
                var definitionsInBlock = content.Mutations.Reverse().Concat(content.Definitions.Reverse()).ToList();

                var definitionsFromKey = definitionsInBlock
                    .Select(GetDefinition)
                    .Where(d => key == d.Key &&
                                definitionsInBlock.Contains(d.Hash))
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