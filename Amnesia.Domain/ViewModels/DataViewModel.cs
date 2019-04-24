using Amnesia.Domain.Entity;

namespace Amnesia.Domain.ViewModels
{
    public class DataViewModel
    {
        public string Hash { get; set; }
        public string PreviousDefinitionHash { get; set; }
        public string Signature { get; set; }

        public DataViewModel(Data data)
        {
            Hash = Model.Hash.ByteArrayToString(data.Blob);
            PreviousDefinitionHash = Model.Hash.ByteArrayToString(data.PreviousDefinitionHash);
            Signature = Model.Hash.ByteArrayToString(data.Signature);
        }
    }
}