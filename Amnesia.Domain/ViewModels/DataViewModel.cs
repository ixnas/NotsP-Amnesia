using Amnesia.Domain.Entity;

namespace Amnesia.Domain.ViewModels
{
    public class DataViewModel
    {
        public string Hash { get; set; }
        public string PreviousDefinitionHash { get; set; }
        public byte[] Signature { get; set; }
        public string Key { get; set; }
        public byte[] Blob { get; set; }

        public static DataViewModel FromData(Data data)
        {
            var vm = new DataViewModel
            {
                Hash = Model.Hash.ByteArrayToString(data.Hash),
                PreviousDefinitionHash = data.PreviousDefinitionHash == null
                                         ? null
                                         : Model.Hash.ByteArrayToString(data.PreviousDefinitionHash),
                Signature = data.Signature,
                Key = data.Key
            };
            return vm;
        }
    }
}