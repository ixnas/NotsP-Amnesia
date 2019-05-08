using System;

namespace Amnesia.Domain.Entity
{
    [Serializable]
    public class State
    {
        public byte[] CurrentBlockHash { get; set; }
        public Block CurrentBlock { get; set; }
    }
}