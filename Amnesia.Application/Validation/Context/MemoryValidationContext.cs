using System.Collections.Generic;
using System.Linq;
using Amnesia.Application.Helper;
using Amnesia.Domain.Entity;

namespace Amnesia.Application.Validation.Context
{
    public class MemoryValidationContext : IValidationContext
    {
        public IDictionary<byte[], Block> Blocks { get; set; } = new Dictionary<byte[], Block>(new ByteArrayEqualityComparer());
        public IDictionary<byte[], Content> Contents { get; set; } = new Dictionary<byte[], Content>(new ByteArrayEqualityComparer());
        public IDictionary<byte[], Definition> Definitions { get; set; } = new Dictionary<byte[], Definition>(new ByteArrayEqualityComparer());
        public IDictionary<byte[], Data> Data { get; set; } = new Dictionary<byte[], Data>(new ByteArrayEqualityComparer());

        public void AddBlock(Block block)
        {
            Blocks.Add(block.Hash, block);
        }

        public void AddContent(Content content)
        {
            Contents.Add(content.Hash, content);
        }

        public void AddDefinition(Definition definition)
        {
            Definitions.Add(definition.Hash, definition);
        }

        public void AddData(Data data)
        {
            Data.Add(data.Hash, data);
        }

        public bool HasBlock(byte[] hash)
        {
            return Blocks.ContainsKey(hash);
        }

        public Block GetBlockAndContent(byte[] hash)
        {
            if (!Blocks.ContainsKey(hash))
            {
                return null;
            }
            
            var block = Blocks[hash];

            if (!Contents.ContainsKey(block.ContentHash))
            {
                return null;
            }

            var content = Contents[block.ContentHash];
            block.Content = content;

            return block;
        }

        public Definition GetDefinition(byte[] hash)
        {
            if (!Definitions.ContainsKey(hash))
            {
                return null;
            }
            
            return Definitions[hash];
        }

        public bool HasDefinition(byte[] hash)
        {
            return Definitions.ContainsKey(hash);
        }

        public IList<Definition> GetDefinitions(byte[] blockHash)
        {
            var block = GetBlockAndContent(blockHash);

            return block.Content.Definitions
                .Select(GetDefinition)
                .ToList();
        }

        public IList<Definition> GetMutations(byte[] blockHash)
        {
            var block = GetBlockAndContent(blockHash);

            return block.Content.Mutations
                .Select(GetDefinition)
                .ToList();
        }

        public Data GetData(byte[] definitionHash)
        {
            var definition = GetDefinition(definitionHash);

            var hash = definition.DataHash;

            return Data.ContainsKey(hash) 
                ? Data[hash]
                : null;
        }

        public IEnumerable<byte[]> GetBlockGraph(byte[] startHash)
        {
            var hash = startHash;
            yield return hash;

            while (true)
            {
                if (!HasBlock(hash))
                {
                    break;
                }

                hash = GetPreviousBlock(hash);

                if (hash == null)
                {
                    break;
                }

                yield return hash;
            }
        }

        public byte[] GetPreviousBlock(byte[] hash)
        {
            return Blocks[hash].PreviousBlockHash;
        }

        public bool ShouldAssumeValid(byte[] blockHash)
        {
            return false;
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