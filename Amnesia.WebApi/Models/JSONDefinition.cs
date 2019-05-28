namespace Amnesia.WebApi.Models
{
    public class JSONDefinition
    {
        public string DataHash;
        public string PreviousDefinitionHash;
        public string Signature;

        public bool IsMutable;
        public bool IsMutation;

        public JSONData Data;
    }
}
