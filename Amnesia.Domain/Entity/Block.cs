using Amnesia.Domain.Model;

namespace Amnesia.Domain.Entity
{
    public class Block : HashableObject
    {
        public byte[] PreviousBlockHash { get; set; }
        public int Nonce { get; set; }
        public byte[] ContentHash { get; set; }

        public Content Content { get; set; }
        public Block PreviousBlock { get; set; }
    }
}
