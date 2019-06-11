using Amnesia.Domain.Entity;

namespace Amnesia.Domain.ViewModels
{
    public class DataViewModel
    {
        public string Hash { get; set; }
        public string PreviousDefinition { get; set; }
        public byte[] Signature { get; set; }
        public string Key { get; set; }
        public byte[] Blob { get; set; }

        public static DataViewModel FromData(Data data, bool includeBlob = false)
        {
            var vm = new DataViewModel
            {
                Hash = Model.Hash.ByteArrayToString(data.Hash),
                PreviousDefinition = data.PreviousDefinitionHash == null
                    ? null
                    : Model.Hash.ByteArrayToString(data.PreviousDefinitionHash),
                Signature = data.Signature,
                Key = data.Key
            };

            if (includeBlob)
            {
                vm.Blob = data.Blob;
            }

            return vm;
        }

        public Data ToData()
        {
            return new Data
            {
                Hash = Model.Hash.StringToByteArray(Hash),
                PreviousDefinitionHash = PreviousDefinition == null ? null : Model.Hash.StringToByteArray(PreviousDefinition),
                Signature = Signature,
                Key = Key,
                Blob = Blob
            };
        }
        
        public bool ShouldSerializeBlob()
        {
            return Blob != null;
        }
    }
}