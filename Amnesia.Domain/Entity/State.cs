using System;

namespace Amnesia.Domain.Entity
{
    public class State
    {
        public string PeerId { get; set; }
        public byte[] CurrentBlockHash { get; set; }
        public Block CurrentBlock { get; set; }
    }
}