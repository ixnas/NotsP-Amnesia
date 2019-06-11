namespace Amnesia.WebApi.Models
{
    public class EasyDefinitionModel
    {
        public byte[] Blob { get; set; }
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }
        public string DefinitionHash { get; set; }
    }
}