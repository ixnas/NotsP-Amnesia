using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Amnesia.Application.Validation.Context;
using Amnesia.Cryptography;
using Amnesia.Domain.Entity;
using Amnesia.Domain.Model;

namespace Amnesia.Application.Validation
{   
    public class BlockValidator
    {
        private readonly IValidationContext context;
        private readonly int difficulty;

        public BlockValidator(IValidationContext context, int difficulty)
        {
            this.context = context;
            this.difficulty = difficulty;
        }

        public ValidationResult ValidateBlock(byte[] hash)
        {
            if (context.ShouldAssumeValid(hash))
            {
                return ValidationResult.Success();
            }

            var block = context.GetBlockAndContent(hash);

            if (block == null || block.Content == null)
            {
                return ValidationResult.Failure($"Block or content {Hash.ByteArrayToString(hash)} could not be found");
            }

            if (!block.Hash.SequenceEqual(hash) ||
                !block.HashObject().SequenceEqual(hash))
            {
                return ValidationResult.Failure($"Block {Hash.ByteArrayToString(hash)} hash does not match");
            }
            
            if (!block.Content.Hash.SequenceEqual(block.ContentHash) ||
                !block.Content.HashObject().SequenceEqual(block.ContentHash))
            {
                return ValidationResult.Failure($"Block {Hash.ByteArrayToString(hash)} content hash does not match");
            }

            var missingData = Enumerable.Empty<byte[]>();

            // if the block is not the genesis block, check the validity of the previous block
            if (block.PreviousBlockHash != null)
            {
                var result = ValidateBlock(block.PreviousBlockHash);

                if (!result.IsSuccess)
                {
                    return result;
                }

                missingData = missingData.Concat(result.MissingData);
            }
            
            if (!ValidateProofOfWork(block.Hash))
            {
                return ValidationResult.Failure($"Proof of work for {Hash.ByteArrayToString(block.Hash)} failed");
            }
            
            foreach (var definition in block.Content.Definitions)
            {
                var result = ValidateDefinition(definition, block.Hash);

                if (!result.IsSuccess)
                {
                    return result;
                }

                missingData = missingData.Concat(result.MissingData);
            }


            return ValidationResult.Success(missingData);
        }

        private bool ValidateProofOfWork(byte[] hash)
        {
            var bigInteger = new BigInteger(hash);
            return (bigInteger & ((1 << difficulty) - 1)) == 0;
        }

        public ValidationResult ValidateDefinition(byte[] hash, byte[] blockHash)
        {
            var definition = context.GetDefinition(hash);

            if (definition == null)
            {
                return ValidationResult.Failure($"definition {Hash.ByteArrayToString(hash)} was not found");
            }

            if (!definition.Hash.SequenceEqual(hash) ||
                !definition.HashObject().SequenceEqual(hash))
            {
                return ValidationResult.Failure($"definition {Hash.ByteArrayToString(hash)} hash does not match");
            }

            var key = new PublicKey(definition.Key);
            if (!key.VerifyData(definition.SignatureHash.EncodeToBytes(), definition.Signature))
            {
                return ValidationResult.Failure($"definition {Hash.ByteArrayToString(hash)} signature is invalid");
            }

            if (definition.IsMutable && definition.IsMutation)
            {
                return ValidationResult.Failure($"definition {Hash.ByteArrayToString(hash)} cannot be mutable and a mutation");
            }

            if (!ValidatePreviousDefinitionHash(definition, blockHash))
            {
                return ValidationResult.Failure($"definition {Hash.ByteArrayToString(hash)} PreviousDefinition is invalid");
            }


            var data = context.GetData(hash);

            if (data == null)
            {
                return ValidationResult.Success(new List<byte[]>{definition.DataHash});
            }

            if (!data.Hash.SequenceEqual(definition.DataHash) ||
                !data.HashObject().SequenceEqual(definition.DataHash))
            {
                return ValidationResult.Failure($"definition {Hash.ByteArrayToString(hash)} data hash does not match");
            }

            if (definition.PreviousDefinitionHash != data.PreviousDefinitionHash)
            {
                return ValidationResult.Failure($"definition {Hash.ByteArrayToString(hash)} PreviousDefinitionHash does not match data");
            }

            if (data.Key != definition.Key)
            {
                return ValidationResult.Failure($"definition {Hash.ByteArrayToString(hash)} Data key does not match key");
            }
            
            if (!key.VerifyData(data.SignatureHash.EncodeToBytes(), data.Signature))
            {
                return ValidationResult.Failure($"definition {Hash.ByteArrayToString(hash)} data signature is invalid");
            }

            return ValidationResult.Success();
        }

        private bool ValidatePreviousDefinitionHash(Definition definition, byte[] blockHash)
        {
            var definitionsFromKey = context.GetDefinitionsByKey(definition.Key, blockHash);
            using var enumerator = definitionsFromKey.GetEnumerator();

            while (enumerator.MoveNext())
            {
                // Find this definition in the sequence
                if (!enumerator.Current.SequenceEqual(definition.Hash))
                {
                    continue;
                }

                // If this is definition is the last in the sequence,
                // it must be the first definition created by this key
                if (!enumerator.MoveNext())
                {
                    return definition.PreviousDefinitionHash == null;
                }

                return definition.PreviousDefinitionHash.SequenceEqual(enumerator.Current);
            }

            return false;
        }
    }
}