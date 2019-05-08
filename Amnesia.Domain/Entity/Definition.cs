using System;
using System.Collections.Generic;
using Amnesia.Domain.Model;

namespace Amnesia.Domain.Entity
{
    [Serializable]
    public class Definition : HashableObject
    {
        public byte[] DataHash { get; set; }
        public byte[] PreviousDefinitionHash { get; set; }
        public byte[] Signature { get; set; }
        public bool IsMutation { get; set; }
        public IDictionary<string, string> Meta { get; set; }
        public Data Data { get; set; }
        public Definition PreviousDefinition { get; set; }
        public byte[] ContentDefinitionHash { get; set; }
        public byte[] ContentMutationHash { get; set; }

        public Definition(byte[] dataHash, byte[] previousDefinitionHash, byte[] signature, bool isMutation, IDictionary<string, string> meta, Data data, Definition previousDefinition, byte[] contentDefinitionHash, byte[] contentMutationHash)
        {
            DataHash = dataHash;
            PreviousDefinitionHash = previousDefinitionHash;
            Signature = signature;
            IsMutation = isMutation;
            Meta = meta;
            Data = data;
            PreviousDefinition = previousDefinition;
            ContentDefinitionHash = contentDefinitionHash;
            ContentMutationHash = contentMutationHash;

            Hash = CalculateSha256Hash();
        }
    }
}
