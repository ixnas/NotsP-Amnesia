namespace Amnesia.Domain.Entity
{
    public class State
    {
        public byte[] CurrentBlockHash { get; set; }
        public Block CurrentBlock { get; set; }
    }
}