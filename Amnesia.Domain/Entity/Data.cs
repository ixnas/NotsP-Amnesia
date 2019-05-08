using System;
using Amnesia.Domain.Model;

namespace Amnesia.Domain.Entity
{
    [Serializable]
    public class Data : HashableObject
    {
        public byte[] PreviousDefinitionHash { get; set; }
        public byte[] Signature { get; set; }
        public byte[] Blob { get; set; }

        public Data(byte[] previousDefinitionHash, byte[] signature, byte[] blob)
        {
            PreviousDefinitionHash = previousDefinitionHash;
            Signature = signature;
            Blob = blob;

            Hash = CalculateSha256Hash();
        }
    }
}
