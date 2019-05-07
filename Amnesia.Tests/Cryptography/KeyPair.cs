using System;
using Amnesia.Cryptography;
using NUnit.Framework;

namespace Amnesia.Tests.Cryptography
{
    [Serializable]
    public class SomeClass
    {
        public string Name;
        public int SomeNumber;
    }

    [TestFixture]
    public class KeyPairTests
    {
        [Test]
        public void KeysAvailable()
        {
            var keyPair = new KeyPair(1024);
            Assert.IsNotNull(keyPair.PrivateKey);
            Assert.IsNotNull(keyPair.PublicKey);
        }

        [Test]
        public void KeyPairFromExistingKeys()
        {
            var keyPair = new KeyPair(1024);
            var newKeyPair = new KeyPair(keyPair.PrivateKey, keyPair.PublicKey);
        }

        [Test]
        public void ValidSignatureFromByteArray()
        {
            var keyPair = new KeyPair(1024);
            var privateKey = keyPair.PrivateKey;
            var publicKey = keyPair.PublicKey;
            byte[] testData = { 0, 2, 4, 2, 1, 6 };

            var signature = privateKey.SignData(testData);
            var isValid = publicKey.VerifyData(testData, signature);

            Assert.IsTrue(isValid);
        }

        [Test]
        public void InvalidSignatureFromByteArray()
        {
            var keyPair = new KeyPair(1024);
            var privateKey = keyPair.PrivateKey;
            var publicKey = keyPair.PublicKey;
            byte[] testData = { 0, 2, 4, 2, 1, 6 };
            byte[] wrongData = { 0, 2, 4, 2, 1, 5 };

            var signature = privateKey.SignData(testData);
            var isValid = publicKey.VerifyData(wrongData, signature);

            Assert.IsFalse(isValid);
        }

        [Test]
        public void ValidSignatureFromObject()
        {
            var keyPair = new KeyPair(1024);
            var privateKey = keyPair.PrivateKey;
            var publicKey = keyPair.PublicKey;
            var testData = new SomeClass
            {
                Name = "Test",
                SomeNumber = 2
            };

            var signature = privateKey.SignObject(testData);
            var isValid = publicKey.VerifyObject(testData, signature);

            Assert.IsTrue(isValid);
        }

        [Test]
        public void InvalidSignatureFromObject()
        {
            var keyPair = new KeyPair(1024);
            var privateKey = keyPair.PrivateKey;
            var publicKey = keyPair.PublicKey;
            var testData = new SomeClass
            {
                Name = "Test",
                SomeNumber = 2
            };

            var wrongData = new SomeClass
            {
                Name = "Test",
                SomeNumber = 3
            };

            var signature = privateKey.SignObject(testData);
            var isValid = publicKey.VerifyObject(wrongData, signature);

            Assert.IsFalse(isValid);
        }
    }
}