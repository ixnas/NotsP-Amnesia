using Amnesia.Domain.Entity;

namespace Amnesia.Domain.ViewModels
{
    public class DataViewModel
    {
        public string Hash { get; set; }
        public string PreviousDefinitionHash { get; set; }
        public string Signature { get; set; }

        public DataViewModel(){}
        public static DataViewModel FromData(Data data)
        {
            var vm = new DataViewModel
            {
                Hash = Model.Hash.ByteArrayToString(data.Hash),
                PreviousDefinitionHash = data.PreviousDefinitionHash == null
                                         ? null
                                         : data.PreviousDefinitionHash,
                Signature = Model.Hash.ByteArrayToString(data.Signature)
            };
            return vm;
        }
    }
}