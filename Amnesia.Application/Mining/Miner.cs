using System;
using Amnesia.Domain.Entity;

namespace Amnesia.Application.Mining
{
    public class Miner
    {
        private readonly int difficulty;

        public event Action<Block> Mined;

        public Miner(int difficulty)
        {
            this.difficulty = difficulty;
        }

        public void Start(Block payload)
        {   
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}