using Amnesia.Domain.Model;

namespace Amnesia.Domain.Entity
{
    public class Data : HashableObject
    {
        public byte[] PreviousDefinitionHash { get; set; }
        public byte[] Signature { get; set; }
        public byte[] Blob { get; set; }
    }
}
