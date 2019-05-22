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

        public DataViewModel(Data data)
        {
            Hash = Model.Hash.ByteArrayToString(data.Blob);
            Signature = data.Signature;
            Key = data.Key;
            Blob = data.Blob;

            if (PreviousDefinitionHash != null)
            {
                PreviousDefinitionHash = Model.Hash.ByteArrayToString(data.PreviousDefinitionHash);
            }
        }
    }
}