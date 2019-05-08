using System;
using Amnesia.Domain.Model;

namespace Amnesia.Domain.Entity
{
    [Serializable]
    public class Block : HashableObject
    {       
        [IncludeInHash]
        public byte[] PreviousBlockHash { get; set; }

        [IncludeInHash]
        public int Nonce { get; set; }

        [IncludeInHash]
        public byte[] ContentHash { get; set; }

        public Content Content { get; set; }
        public Block PreviousBlock { get; set; }
    }
}
