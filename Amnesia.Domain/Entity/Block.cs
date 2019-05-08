using System;
using System.Security.Cryptography;
using System.Text;
using Amnesia.Cryptography;
using Amnesia.Domain.Model;

namespace Amnesia.Domain.Entity
{
    [Serializable]
    public class Block : HashableObject
    {       
        public byte[] PreviousBlockHash { get; set; }
        public int Nonce { get; set; }
        public byte[] ContentHash { get; set; }
        public Content Content { get; set; }
        public Block PreviousBlock { get; set; }

        public Block(int nonce, byte[] contentHash, Content content, Block previousBlock = null,
            byte[] previousBlockHash = null)
        {
            PreviousBlockHash = previousBlockHash;
            Nonce = nonce;
            ContentHash = contentHash;
            Content = content;
            PreviousBlock = previousBlock;
            
            Hash = CalculateSha256Hash();
        }
    }
}
