using System.Collections.Generic;
using Amnesia.Domain.Entity;

namespace Amnesia.Application.Validation.Context
{
    public interface IValidationContext
    {
        bool HasBlock(byte[] hash);

        Block GetBlockAndContent(byte[] hash);

        byte[] GetPreviousBlock(byte[] hash);

        Definition GetDefinition(byte[] hash);

        bool HasDefinition(byte[] hash);

        IList<Definition> GetDefinitions(byte[] blockHash);

        IList<Definition> GetMutations(byte[] blockHash);

        Data GetData(byte[] definitionHash);

        IEnumerable<byte[]> GetBlockGraph(byte[] startHash);

        IList<Definition> MissingData { get; set; }

        bool ShouldAssumeValid(byte[] blockHash);

        IEnumerable<byte[]> GetDefinitionsByKey(string key, byte[] startBlock);
    }
}