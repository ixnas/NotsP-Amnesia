using System.Runtime.InteropServices;
using Amnesia.Application.Mining;
using Amnesia.Domain.Entity;
using Amnesia.Domain.Model;
using NUnit.Framework;

namespace Amnesia.Tests.Mining
{
    [TestFixture]
    public class MiningTest
    {
        private Miner miner = new Miner(1);

        [Test]
        public void TestHashingOfBlock()
        {
            var data = new Data
            {
                PreviousDefinitionHash = null,
                Signature = Hash.StringToByteArray("Handtekening"),
                Blob = Hash.StringToByteArray("Dit is een stuk data.")
            };
            
            var definition = new Definition
            {
                DataHash = "",
                PreviousDefinitionHash = null,
                Signature = data.Signature,
                PreviousDefinition = 
            };
            
            var block = new Block
            {
                PreviousBlockHash = Hash.StringToByteArray("0"),
                Nonce = 0,
                Content = null,
                ContentHash = Hash.StringToByteArray("0"),
                PreviousBlock = null
            };
            
//        public byte[] DataHash { get; set; }
//        public byte[] PreviousDefinitionHash { get; set; }
//        public byte[] Signature { get; set; }
//        public bool IsMutation { get; set; }
//        public IDictionary<string, string> Meta { get; set; }
//        public Data Data { get; set; }
//        public Definition PreviousDefinition { get; set; }
//        public byte[] ContentDefinitionHash { get; set; }
//        public byte[] ContentMutationHash { get; set; }
//            
            
            
            
            
            var content = new Content();
        }
        
    }
}