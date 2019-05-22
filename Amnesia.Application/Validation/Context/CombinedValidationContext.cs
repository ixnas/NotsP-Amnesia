using System;
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

            do
            {
                yield return hash;
                hash = GetPreviousBlock(hash);
            } while (hash != null);
        }

        public byte[] GetPreviousBlock(byte[] hash)
        {
            return this
                .Select(c => c.GetPreviousBlock(hash))
                .FirstOrDefault(h => h != null);
        }

        public IList<Definition> MissingData
        {
            get => this.Aggregate(Enumerable.Empty<Definition>(), (acc, c) => acc.Concat(c.MissingData)).ToList();
            set => throw new NotSupportedException();
        }

        public bool ShouldAssumeValid(byte[] blockHash)
        {
            return this.Any(c => c.ShouldAssumeValid(blockHash));
        }
    }
}