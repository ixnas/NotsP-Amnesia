using System;
using System.Collections.Generic;
using Amnesia.Domain.Model;

namespace Amnesia.Domain.Entity
{
    [Serializable]
    public class Content : HashableObject
    {
        public IList<Definition> Definitions { get; set; }
        public IList<Definition> Mutations { get; set; }
        public Block Block { get; set; }
        
        public Content(IList<Definition> definitions, IList<Definition> mutations, Block block)
        {
            Definitions = definitions;
            Mutations = mutations;
            Block = block;

            Hash = CalculateSha256Hash();
        }
    }
}
