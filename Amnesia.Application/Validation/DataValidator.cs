using System.Threading.Tasks;
using Amnesia.Application.Services;
using Amnesia.Cryptography;
using Amnesia.Domain.Entity;

namespace Amnesia.Application.Validation
{
    public class DataValidator
    {
        readonly DefinitionService definitionService;
        readonly DefinitionValidator definitionValidator;

        public DataValidator(DefinitionService definitionService, DefinitionValidator definitionValidator)
        {
            this.definitionService = definitionService;
            this.definitionValidator = definitionValidator;
        }

        async Task<bool> PreviousDefinitionValid(Data data)
        {
            if (data.PreviousDefinitionHash == null)
            {
                return true;
            }

            var definition = await definitionService.GetDefinition(data.PreviousDefinitionHash);

            if (definition == null)
            {
                return false;
            }

            return definitionValidator.Validate(definition);
        }

        bool SignatureIsValid(Data data)
        {
            var key = new PublicKey(data.Key);
            return key.VerifyData(data.SignatureHash.EncodeToBytes(), data.Signature);
        }

        public async Task<bool> Validate(Data data)
        {
            return await PreviousDefinitionValid(data) && SignatureIsValid(data);
        }
    }
}