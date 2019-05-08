using System;
using Amnesia.Domain.Model;

namespace Amnesia.Domain.Entity
{
    [Serializable]
    public class Data : HashableObject
    {
        [IncludeInHash]
        public byte[] PreviousDefinitionHash { get; set; }
        [IncludeInHash]
        public byte[] Signature { get; set; }
        [IncludeInHash]
        public byte[] Blob { get; set; }

        [Obsolete("Useless constructor is useless, remove asap, use Object Initialization")]
        public Data(byte[] previousDefinitionHash, byte[] signature, byte[] blob)
        {
            PreviousDefinitionHash = previousDefinitionHash;
            Signature = signature;
            Blob = blob;
        }
    }
}
