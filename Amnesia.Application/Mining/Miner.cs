using System;
using System.Collections.Generic;
using Amnesia.Domain.Entity;

namespace Amnesia.Application.Mining
{
    public class Miner
    {
        private readonly int difficulty;

        public Miner(int difficulty)
        {
            this.difficulty = difficulty;
        }

        public ICollection<Definition> Payload { get; private set; } = new List<Definition>();

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}