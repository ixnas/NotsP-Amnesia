using System;
using Amnesia.Domain.Model;

namespace Amnesia.Domain.Entity
{
    public class Data : HashableObject
    {
        public byte[] PreviousDefinitionHash { get; set; }
        public byte[] Signature { get; set; }
        public string Key { get; set; }
        public byte[] Blob { get; set; }

        public override CompositeHash PrimaryHash => new CompositeHash(this)
            .Add(nameof(PreviousDefinitionHash))
            .Add(nameof(Blob))
            .Add(nameof(Signature))
            .Add(nameof(Key));

        public CompositeHash SignatureHash => new CompositeHash(this)
            .Add(nameof(PreviousDefinitionHash))
            .Add(nameof(Blob));
    }
}
