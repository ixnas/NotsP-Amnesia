using System;
using System.Collections.Generic;
using Amnesia.Domain.Model;

namespace Amnesia.Domain.Entity
{
    public class Definition : HashableObject
    {
        public byte[] DataHash { get; set; }
        public byte[] PreviousDefinitionHash { get; set; }
        public byte[] Signature { get; set; }
        public byte[] Key { get; set; }
        public bool IsMutation { get; set; }
        public bool IsMutable { get; set; }
        public Data Data { get; set; }
        public Definition PreviousDefinition { get; set; }

        public CompositeHash SignatureHash => new CompositeHash(this)
            .Add(nameof(DataHash))
            .Add(nameof(PreviousDefinitionHash))
            .Add(nameof(IsMutation))
            .Add(nameof(IsMutable));

        public override CompositeHash PrimaryHash => new CompositeHash(this)
            .Add(nameof(DataHash))
            .Add(nameof(PreviousDefinitionHash))
            .Add(nameof(IsMutation))
            .Add(nameof(IsMutable))
            .Add(nameof(Signature))
            .Add(nameof(Key));
    }
}
