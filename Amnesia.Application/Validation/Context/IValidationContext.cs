using System.Collections.Generic;
using Amnesia.Domain.Entity;

namespace Amnesia.Application.Validation.Context
{
    public interface IValidationContext
    {
        /// <summary>
        /// Checks if a block is known in the context
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        bool HasBlock(byte[] hash);

        /// <summary>
        /// Returns the block with given hash and the associated content
        /// </summary>
        /// <param name="hash"></param>
        /// <returns>Block (with Content property) or null</returns>
        Block GetBlockAndContent(byte[] hash);

        byte[] GetPreviousBlock(byte[] hash);

        Definition GetDefinition(byte[] hash);

        bool HasDefinition(byte[] hash);

        IList<Definition> GetDefinitions(byte[] blockHash);

        IList<Definition> GetMutations(byte[] blockHash);

        Data GetData(byte[] definitionHash);

        /// <summary>
        /// Returns an enumeration of block hashes
        /// where each block references the next block as their PreviousBlock.
        /// </summary>
        /// <remarks>
        /// Starts with <paramref name="startHash"/> regardless of whether it is found or not.
        /// Ends if the previousBlock cannot be found or the previousBlock is null or not found.
        /// The last item could be the genesis block.
        /// The last item could be the previousBlock that was not found in the current context.
        /// </remarks>
        /// <param name="startHash">The hash of the block that starts the graph</param>
        /// <returns></returns>
        IEnumerable<byte[]> GetBlockGraph(byte[] startHash);

        bool ShouldAssumeValid(byte[] blockHash);

        IEnumerable<byte[]> GetDefinitionsByKey(string key, byte[] startBlock);
    }
}