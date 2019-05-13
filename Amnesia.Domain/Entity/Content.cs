using System;
using System.Collections.Generic;
using Amnesia.Domain.Model;

namespace Amnesia.Domain.Entity
{
    public class Content : HashableObject
    {
        public IList<byte[]> Definitions { get; set; }
        public IList<byte[]> Mutations { get; set; }
        public Block Block { get; set; }
        public override CompositeHash PrimaryHash => new CompositeHash(this)
            .Add(nameof(Definitions))
            .Add(nameof(Mutations));
    }
}
