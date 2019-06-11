namespace Amnesia.WebApi.Models
{
    public class AddDataViewModel
    {
        public string Hash { get; set; }
        public string PreviousDefinition { get; set; }
        public byte[] Signature { get; set; }
        public string Key { get; set; }
        public byte[] Blob { get; set; }
    }
}