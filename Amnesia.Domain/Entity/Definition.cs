using System.Collections.Generic;
using Amnesia.Domain.Model;

namespace Amnesia.Domain.Entity
{
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
    }
}
