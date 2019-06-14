using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Amnesia.Application.Validation.Context;
using Amnesia.Application.Validation.Result;
using Amnesia.Cryptography;
using Amnesia.Domain.Entity;
using Amnesia.Domain.Model;

namespace Amnesia.Application.Validation
{
    public class DefinitionValidator
    {
        private readonly IValidationContext context;

        public DefinitionValidator(IValidationContext context)
        {
            this.context = context;
        }

        public IDefinitionValidationResult ValidateDefinition(byte[] hash, byte[] blockHash)
        {
            var definition = context.GetDefinition(hash);

            if (definition == null)
            {
                return new DefinitionFailureResult($"definition {Hash.ByteArrayToString(hash)} was not found");
            }

            if (!definition.Hash.SequenceEqual(hash) ||
                !definition.HashObject().SequenceEqual(hash))
            {
                return new DefinitionFailureResult($"definition {Hash.ByteArrayToString(hash)} hash does not match");
            }

            var key = new PublicKey(definition.Key);
            if (!key.VerifyData(definition.SignatureHash.EncodeToBytes(), definition.Signature))
            {
                return new DefinitionFailureResult($"definition {Hash.ByteArrayToString(hash)} signature is invalid");
            }

            if (definition.IsMutable && definition.IsMutation)
            {
                return new DefinitionFailureResult($"definition {Hash.ByteArrayToString(hash)} cannot be mutable and a mutation");
            }

            if (!ValidatePreviousDefinitionHash(definition, blockHash))
            {
                return new DefinitionFailureResult($"definition {Hash.ByteArrayToString(hash)} PreviousDefinition is invalid");
            }


            var data = context.GetData(hash);

            if (data == null)
            {
                if (!definition.IsMutable)
                    return new DefinitionFailureResult(
                        $"definition {Hash.ByteArrayToString(hash)} is not mutable but data was not found");

                return new DefinitionMissingDataResult();
            }

            if (!data.Hash.SequenceEqual(definition.DataHash) ||
                !data.HashObject().SequenceEqual(definition.DataHash))
            {
                return new DefinitionFailureResult($"definition {Hash.ByteArrayToString(hash)} data hash does not match");
            }

            if (definition.PreviousDefinitionHash != data.PreviousDefinitionHash &&
                !definition.PreviousDefinitionHash.SequenceEqual(data.PreviousDefinitionHash))
            {
                return new DefinitionFailureResult($"definition {Hash.ByteArrayToString(hash)} PreviousDefinitionHash does not match data");
            }

            if (data.Key != definition.Key)
            {
                return new DefinitionFailureResult($"definition {Hash.ByteArrayToString(hash)} Data key does not match key");
            }
            
            if (!key.VerifyData(data.SignatureHash.EncodeToBytes(), data.Signature))
            {
                return new DefinitionFailureResult($"definition {Hash.ByteArrayToString(hash)} data signature is invalid");
            }

            if (definition.IsMutation)
            {
                return ValidateMutation(hash,definition, data);
            }

            return new DefinitionSuccessResult();
        }

        private IDefinitionValidationResult ValidateMutation(byte[] hash, Definition definition, Data data)
        {
            var referencingHash = ParseMutationHash(data.Blob);

            if (referencingHash == null)
            {
                return new DefinitionFailureResult($"mutation {Hash.ByteArrayToString(hash)} blob could not be parsed");
            }

            var referencingDefinition = context.GetDefinition(referencingHash);

            if (referencingDefinition == null)
            {
                return new DefinitionFailureResult($"mutation {Hash.ByteArrayToString(hash)} refers to a definition that does not exist");
            }

            if (referencingDefinition.Key != definition.Key)
            {
                return new DefinitionFailureResult($"mutation {Hash.ByteArrayToString(hash)} refers to a definition with a different owner");
            }

            if (!referencingDefinition.IsMutable)
            {
                return new DefinitionFailureResult($"mutation {Hash.ByteArrayToString(hash)} refers to a definition that is not mutable");
            }

            return new DefinitionDeletedDataResult(referencingHash);
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

        private static byte[] ParseMutationHash(byte[] blob)
        {
            try
            {
                var str = Encoding.UTF8.GetString(blob);

                var regex = new Regex("DELETE ([0-9a-fA-F]+)");
                var match = regex.Match(str);

                if (!match.Success)
                {
                    return null;
                }

                var hash = match.Groups[1].Value;

                return Hash.StringToByteArray(hash);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}