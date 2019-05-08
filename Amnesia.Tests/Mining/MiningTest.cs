using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using Amnesia.Application.Mining;
using Amnesia.Cryptography;
using Amnesia.Domain.Entity;
using Amnesia.Domain.Model;
using NUnit.Framework;
using PeterO.Cbor;

namespace Amnesia.Tests.Mining
{
    [TestFixture]
    public class MiningTest
    {
        private Serializer serializer = new Serializer();

        [Test]
        public void TestHashingOfBlock()
        {
            var data = new Data(null,
                serializer.Serialize("Handtekening"),
                serializer.Serialize("Dit is een stuk data"));

            var definition = new Definition(data.Hash, null, data.Signature,
                false, null, data, null, null, null);

            var list = new List<Definition> {definition};

            var content = new Content(list, null, null);

            var block = new Block();

            //aef7925610ce72001910b0bf4674793669cd6fe0fbc706961522ac2fbaeb492b - 0
            //8a9f9dcdf4dbefd8a3108dbb99ca4f05fd93e483fccdf3ee092964038dc481aa - 1
            //69361095198ec77b33d1594303eadd5c478748ad57495a4a484b96e42c565f1c - 2


            Console.WriteLine("Hash: {0}", Hash.ByteArrayToString(block.Hash));
        }

        [Test]
        public void TestConversion()
        {
            var content = new Content(null, null, null);

            var block = new Block();

            var cbor = CBORObject.NewMap()
                .Add("ContentHash", content.Hash)
                .Add("PreviousBlockHash", new byte[] {0})
                .Add("Nonce", 0);

            Console.WriteLine(cbor.ToJSONString());
            
            byte[] bytes = cbor.EncodeToBytes();
            string json = cbor.ToJSONString();

            var sha256bytes = CalculateSha256Hash(bytes);
            
            Console.WriteLine(Calculate256HashString(CalculateSha256Hash(bytes)));

            var t = CBORObject.DecodeFromBytes(bytes);
            
            Console.WriteLine(t.ToJSONString());
        }
        
        protected byte[] CalculateSha256Hash(byte[] cbor)
        {   
            using (SHA256 sha256Hash = SHA256.Create())  
            {  
                byte[] bytes = sha256Hash.ComputeHash(cbor);
                return bytes;
            }  
        }
        
        public string Calculate256HashString(byte[] sha256)
        {
            StringBuilder builder = new StringBuilder();  
            for (int i = 0; i < sha256.Length; i++)  
            {  
                builder.Append(sha256[i].ToString("x2"));  
            }  
            return builder.ToString();
        }
    }
}