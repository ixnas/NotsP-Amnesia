using Amnesia.Cryptography;
using Amnesia.Domain.Entity;

namespace Amnesia.Application.Validation
{
    public class DefinitionValidator
    {
        public DefinitionValidator()
        {
        }

        bool DataExists(Definition definition)
        {
            throw new System.NotImplementedException();
        }

        bool MissingDataWasMutated(Definition definition)
        {
            throw new System.NotImplementedException();
        }

        bool SignatureIsValid(Definition definition)
        {
            var key = new PublicKey(definition.Key);
            return key.VerifyData(definition.SignatureHash.EncodeToBytes(), definition.Signature);
        }

        public bool Validate(Definition definition)
        {
            throw new System.NotImplementedException();
        }
    }
}