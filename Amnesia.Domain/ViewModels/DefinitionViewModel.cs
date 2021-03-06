using Amnesia.Domain.Entity;

namespace Amnesia.Domain.ViewModels
{
    public class DefinitionViewModel
    {
        public string Hash { get; set; }
        public string DataHash { get; set; }
        public bool IsMutation { get; set; }
        public bool IsMutable { get; set; }
        public string PreviousDefinition { get; set; }
        public byte[] Signature { get; set; }
        public string Key { get; set; }

        public static DefinitionViewModel FromDefinition(Definition definition)
        {
            var vm = new DefinitionViewModel
            {
                Hash = Model.Hash.ByteArrayToString(definition.Hash),
                DataHash = Model.Hash.ByteArrayToString(definition.DataHash),
                PreviousDefinition = definition.PreviousDefinitionHash == null
                                     ? null
                                     : Model.Hash.ByteArrayToString(definition.PreviousDefinitionHash),
                Signature = definition.Signature,
                IsMutable = definition.IsMutable,
                IsMutation = definition.IsMutation,
                Key = definition.Key
            };
            return vm;
        }

        public Definition ToDefinition()
        {
            return new Definition
            {
                Hash = Model.Hash.StringToByteArray(Hash),
                DataHash = Model.Hash.StringToByteArray(DataHash),
                IsMutable = IsMutable,
                IsMutation = IsMutation,
                PreviousDefinitionHash = PreviousDefinition == null ? null : Model.Hash.StringToByteArray(PreviousDefinition),
                Signature = Signature,
                Key = Key
            };       
        }
    }
}