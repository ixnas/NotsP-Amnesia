using System;
using Amnesia.Domain.Entity;
using Amnesia.Domain.Model;
using NUnit.Framework;

namespace Amnesia.Tests.Domain
{
    public class HashTests
    {
        [Test]
        public void ShouldHashBlock()
        {
            var block = new Block
            {
                ContentHash = Hash.StringToByteArray("abcde0"),
                PreviousBlockHash = Hash.StringToByteArray("123456"),
                Nonce = 1
            };

            var hash = block.Hash;
            var hashString = Hash.ByteArrayToString(hash);
            
            Assert.AreEqual("20c96b6e607263caa63828b4eae68c0f935f394218c485ff0580a8781b1d28f4", hashString);
        }

        [Test]
        public void TwoBlocksWithSamePropertiesShouldHaveSameHashes()
        {
            var a = new Block
            {
                ContentHash = Hash.StringToByteArray("abcde0"),
                PreviousBlockHash = Hash.StringToByteArray("123456"),
                Nonce = 1
            };

            var b = new Block
            {
                ContentHash = Hash.StringToByteArray("abcde0"),
                PreviousBlockHash = Hash.StringToByteArray("123456"),
                Nonce = 1
            };

            var hashA = Hash.ByteArrayToString(a.Hash);
            var hashB = Hash.ByteArrayToString(b.Hash);

            Assert.AreEqual(hashA, hashB);
        }

        [Test]
        public void TwoBlocksWithDifferentPropertiesShouldHaveDifferentHashes()
        {
            var a = new Block
            {
                ContentHash = Hash.StringToByteArray("abcde0"),
                PreviousBlockHash = Hash.StringToByteArray("123456"),
                Nonce = 1
            };

            var b = new Block
            {
                ContentHash = Hash.StringToByteArray("37103462"),
                PreviousBlockHash = Hash.StringToByteArray("70434726"),
                Nonce = 1
            };

            var hashA = Hash.ByteArrayToString(a.Hash);
            var hashB = Hash.ByteArrayToString(b.Hash);

            Assert.AreNotEqual(hashA, hashB);
        }

        [Test]
        public void NavigationalPropertiesShouldBeIgnored()
        {
            var a = new Block
            {
                ContentHash = Hash.StringToByteArray("abcde0"),
                PreviousBlockHash = Hash.StringToByteArray("123456"),
                Nonce = 1,
                Content = new Content()
            };

            var b = new Block
            {
                ContentHash = Hash.StringToByteArray("abcde0"),
                PreviousBlockHash = Hash.StringToByteArray("123456"),
                Nonce = 1,
                Content = null
            };

            var hashA = Hash.ByteArrayToString(a.Hash);
            var hashB = Hash.ByteArrayToString(b.Hash);

            Assert.AreEqual(hashA, hashB);
        }

        [Test]
        public void SignatureHashShouldBeDifferentFromPrimaryHash()
        {
            var data = new Data
            {
                Blob = Hash.StringToByteArray("abcde0"),
                PreviousDefinitionHash = Hash.StringToByteArray("123456"),
                Signature = Hash.StringToByteArray("123412341234"),
                Key = "0987654321"
            };

            var hash = Hash.ByteArrayToString(data.Hash);
            var primaryHash = Hash.ByteArrayToString(data.PrimaryHash.Hash);
            var signatureHash = Hash.ByteArrayToString(data.SignatureHash.Hash);

            Assert.AreEqual(hash, primaryHash);
            Assert.AreNotEqual(hash, signatureHash);
        }
    }
}