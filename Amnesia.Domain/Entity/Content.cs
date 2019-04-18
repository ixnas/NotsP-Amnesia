using System.Collections.Generic;
using Amnesia.Domain.Model;

namespace Amnesia.Domain.Entity
{
    public class Content : HashableObject
    {
        public IList<Definition> Definitions { get; set; }
        public IList<Definition> Mutations { get; set; }
        public Block Block { get; set; }
    }
}
