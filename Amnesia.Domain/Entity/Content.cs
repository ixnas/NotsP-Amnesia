using System;
using System.Collections.Generic;
using Amnesia.Domain.Model;

namespace Amnesia.Domain.Entity
{
    public class Content : HashableObject
    {
        public IList<byte[]> Definitions { get; set; } = new List<byte[]>();
        public IList<byte[]> Mutations { get; set; } = new List<byte[]>();
        public Block Block { get; set; }

        public override CompositeHash PrimaryHash => new CompositeHash(this)
            .Add(nameof(Definitions))
            .Add(nameof(Mutations));

        public Content()
        {
            Mutations = new List<byte[]>();
            Definitions = new List<byte[]>();
        }
    }
}
