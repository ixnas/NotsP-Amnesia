using System;
using System.Collections.Generic;
using Amnesia.Domain.Model;

namespace Amnesia.Domain.Entity
{
    [Serializable]
    public class Definition : HashableObject
    {
        [IncludeInHash]
        public byte[] DataHash { get; set; }
        [IncludeInHash]
        public byte[] PreviousDefinitionHash { get; set; }
        [IncludeInHash]
        public byte[] Signature { get; set; }
        [IncludeInHash]
        public bool IsMutation { get; set; }
        [IncludeInHash]
        public IDictionary<string, string> Meta { get; set; }
        public Data Data { get; set; }
        public Definition PreviousDefinition { get; set; }
        public byte[] ContentDefinitionHash { get; set; }
        public byte[] ContentMutationHash { get; set; }

        [Obsolete("Useless constructor is useless, remove asap, use Object Initialization")]
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
        }
    }
}
