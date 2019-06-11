using System.Linq;
using System.Numerics;
using Amnesia.Application.Validation.Context;
using Amnesia.Application.Validation.Result;
using Amnesia.Domain.Model;

namespace Amnesia.Application.Validation
{   
    public class BlockValidator
    {
        private readonly IValidationContext context;
        private readonly int difficulty;
        private readonly DefinitionValidator definitionValidator;

        public BlockValidator(IValidationContext context, int difficulty)
        {
            this.context = context;
            this.difficulty = difficulty;
            definitionValidator = new DefinitionValidator(context);
        }

        public IBlockValidationResult ValidateBlock(byte[] hash)
        {
            if (context.ShouldAssumeValid(hash))
            {
                return new BlockSuccessResult();
            }

            var block = context.GetBlockAndContent(hash);

            if (block == null || block.Content == null)
            {
                return new BlockFailureResult($"Block or content {Hash.ByteArrayToString(hash)} could not be found");
            }

            if (!block.Hash.SequenceEqual(hash) ||
                !block.HashObject().SequenceEqual(hash))
            {
                return new BlockFailureResult($"Block {Hash.ByteArrayToString(hash)} hash does not match");
            }
            
            if (!block.Content.Hash.SequenceEqual(block.ContentHash) ||
                !block.Content.HashObject().SequenceEqual(block.ContentHash))
            {
                return new BlockFailureResult($"Block {Hash.ByteArrayToString(hash)} content hash does not match");
            }

            var missingData = Enumerable.Empty<byte[]>();

            // if the block is not the genesis block, check the validity of the previous block
            if (block.PreviousBlockHash != null)
            {
                var result = ValidateBlock(block.PreviousBlockHash);

                if (result is BlockFailureResult)
                {
                    return result;
                }

                if (result is BlockAcceptableResult acceptable)
                {
                    missingData = missingData.Concat(acceptable.MissingData);
                }
            }
            
            if (!ValidateProofOfWork(block.Hash))
            {
                return new BlockFailureResult($"Proof of work for {Hash.ByteArrayToString(block.Hash)} failed");
            }
            
            foreach (var definition in block.Content.Definitions.Concat(block.Content.Mutations))
            {
                var result = definitionValidator.ValidateDefinition(definition, block.Hash);

                if (result is DefinitionFailureResult failure)
                {
                    return new BlockFailureResult(failure.Message);
                }

                if (result is DefinitionMissingDataResult)
                {
                    missingData = missingData.Append(definition);
                }

                if (result is DefinitionDeletedDataResult deleted)
                {
                    missingData = missingData.Where(h => !h.SequenceEqual(deleted.ReferencingDefinition));
                }
            }

            if (missingData.Any())
            {
                return new BlockAcceptableResult(missingData);
            }

            return new BlockSuccessResult();
        }

        private bool ValidateProofOfWork(byte[] hash)
        {
            var bigInteger = new BigInteger(hash);
            return (bigInteger & ((1 << difficulty) - 1)) == 0;
        }
    }
}