using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Amnesia.Application.Mining;
using Amnesia.Cryptography;
using Amnesia.Domain.Entity;
using Amnesia.Domain.Model;
using NUnit.Framework;

namespace Amnesia.Tests.Mining
{
    [TestFixture]
    public class MiningTest
    {
        [Test]
        public async Task TestMiningWithDifficultyOfTen()
        {
            var timer = new Stopwatch();
            timer.Start();
            
            var miner = new Miner(10);
            var keys = new KeyPair(2048);
            var block = MakeBlock(MakeContent(MakeDefinition(keys, MakeData(keys))));
            
            miner.Mined += b =>
            {
                Assert.AreEqual(b.HashObject(), block.HashObject());
                Assert.AreEqual(b.Nonce, block.Nonce);
            };
            
            await miner.Start(block);

            Assert.True(Miner.CheckHash(block.Hash, 10));
        }

        // MaxTime does not cancel the test, timeout not implemented in .net standard
        [Test, MaxTime(1000)]
        public void TestStopMining()
        {
            var miner = new Miner(30);
            var block = new Block();
            var task = miner.Start(block);
            miner.Stop();
            Assert.ThrowsAsync(Is.InstanceOf<Exception>(), () => task);
            Assert.True(miner.cancellationTokenSource.IsCancellationRequested);
        }

        //Het enige wat deze test doet is kijken hoe lang elke diffulty duurt
        //Een diff van 20 is iets langer dan een minuut, een diff van 22 is al bijna 10 minuten.
        [Test]
        [Explicit("This mining test is too expensive to run every time")]
        public async Task TestExecuteTimeOfMultipleDifficulties()
        {
            var timer = new Stopwatch();
            var keys = new KeyPair(2048);
            var block = MakeBlock(MakeContent(MakeDefinition(keys, MakeData(keys))));

            for (var difficulty = 0; difficulty <= 20; difficulty++)
            {
                var miner = new Miner(difficulty);
                block.Nonce = 0;

                timer.Restart();

                await miner.Start(block);

                timer.Stop();

                TestContext.WriteLine($"Difficulty: \t{difficulty}");
                TestContext.WriteLine($"Time: \t{timer.Elapsed:g}");
                TestContext.WriteLine($"Hash: \t{Hash.ByteArrayToString(block.Hash)}");
                TestContext.WriteLine("-----------");

                Assert.True(Miner.CheckHash(block.Hash, difficulty));
            }
        }

        [TestCase("f41201ce27", 0, true)]
        [TestCase("941201ce27", 2, true)]
        [TestCase("941201ce27", 3, false)]
        [TestCase("60dc4abe82", 5, true)]
        [TestCase("60dc4abe82", 6, false)]
        [TestCase("3e2dcbc689", 4, false)]
        [TestCase("0000b0e06a", 20, true)]
        [TestCase("0000b0e06a", 21, false)]
        [TestCase("0000b0e06a", 22, false)]
        public void ShouldTestCorrectDifficulty(string hash, int difficulty, bool expected)
        {
            var hashBytes = Hash.StringToByteArray(hash);

            var actual = Miner.CheckHash(hashBytes, difficulty);

            var bitArray = new BitArray(hashBytes);
            foreach (bool b in bitArray)
            {
                TestContext.Write(b ? "1":"0");
            }
            TestContext.WriteLine();

            Assert.AreEqual(expected, actual);
        }
        
        private Data MakeData(KeyPair keys)
        {
            var blob = Encoding.UTF8.GetBytes("Dit is test data.");
            var signature = keys.PrivateKey.SignData(blob);
            
            return new Data
            {
                PreviousDefinitionHash = null,
                Signature = signature,
                Key = keys.PublicKey.ToPEMString(),
                Blob = blob
            };
        }

        private Definition MakeDefinition(KeyPair keys, Data data)
        {
            var definition = new Definition
            {
                DataHash = data.Hash,
                PreviousDefinitionHash = null,
                Key = keys.PublicKey.ToPEMString(),
                IsMutation = false,
                IsMutable = true,
                Data = data,
                PreviousDefinition = null
            };
            var signature = keys.PrivateKey.SignData(definition.SignatureHash.Hash);
            definition.Signature = signature;
            return definition;
        }

        private Content MakeContent(Definition definition)
        {
            var definitions = new List<byte[]> {definition.Hash};
            var mutations = new List<byte[]>();
            return new Content
            {
                Definitions = definitions,
                Mutations = mutations
            };
        }

        private Block MakeBlock(Content content)
        {
            return new Block
            {
                PreviousBlockHash = null,
                Nonce = 0,
                Content = content,
                ContentHash = content.Hash,
                PreviousBlock = null
            };
        }
    }
}