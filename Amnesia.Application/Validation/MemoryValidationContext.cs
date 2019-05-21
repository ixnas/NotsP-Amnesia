using System.Collections.Generic;
using System.Linq;
using Amnesia.Domain.Entity;

namespace Amnesia.Application.Validation
{
    public class MemoryValidationContext : IValidationContext
    {
        public IDictionary<byte[], Block> Blocks { get; set; } = new Dictionary<byte[], Block>();
        public IDictionary<byte[], Content> Contents { get; set; } = new Dictionary<byte[], Content>();
        public IDictionary<byte[], Definition> Definitions { get; set; } = new Dictionary<byte[], Definition>();
        public IDictionary<byte[], Data> Data { get; set; } = new Dictionary<byte[], Data>();

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
            var block = Blocks[hash];

            var content = Contents[block.ContentHash];
            block.Content = content;

            return block;
        }

        public Definition GetDefinition(byte[] hash)
        {
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

            return Data[hash];
        }

        public IEnumerable<byte[]> GetBlockGraph(byte[] startHash)
        {
            var hash = startHash;

            do
            {
                yield return hash;
                hash = Blocks[hash].PreviousBlockHash;
            } while (hash != null);
        }

        public IList<Definition> MissingData { get; set; }
    }
}