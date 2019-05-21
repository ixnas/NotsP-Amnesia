using System.Collections.Generic;
using Amnesia.Application.Services;
using Amnesia.Domain.Entity;

namespace Amnesia.Application.Validation
{
    public interface IValidationContext
    {
        bool HasBlock(byte[] hash);

        Block GetBlockAndContent(byte[] hash);

        Definition GetDefinition(byte[] hash);

        bool HasDefinition(byte[] hash);

        IList<Definition> GetDefinitions(byte[] blockHash);

        IList<Definition> GetMutations(byte[] blockHash);

        Data GetData(byte[] definitionHash);

        IEnumerable<byte[]> GetBlockGraph(byte[] startHash);

        IList<Definition> MissingData { get; set; }
    }
}