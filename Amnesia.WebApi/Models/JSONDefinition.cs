namespace Amnesia.WebApi.Models
{
    public class JSONDefinition
    {
        public string Hash;
        public string PreviousDefinitionHash;
        public string Signature;

        public bool IsMutable;
        public bool IsMutation;

        public JSONData Data;
    }
}
