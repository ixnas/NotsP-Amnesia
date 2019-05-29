using System;
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
        }

        //Does not cancel the test, timeout not implemented in .net standard
        [Test, MaxTime(1000)]
        public void TestStopMining()
        {
            var miner = new Miner(30);
            var block = new Block();
            var task = miner.Start(block);
            miner.Stop();
            Assert.ThrowsAsync<OperationCanceledException>(() => task);
            Assert.True(miner.cancellationTokenSource.IsCancellationRequested);
        }

        //Het enige wat deze test doet is kijken hoe lang elke diffulty duurt
        //Een diff van 20 is iets langer dan een minuut, een diff van 22 is al bijna 10 minuten.
        [Test]
        public async Task TestExecuteTimeOfMultipleDifficulties()
        {
            var timer = new Stopwatch();
            timer.Start();
            var keys = new KeyPair(2048);

            for (int i = 0; i <= 20; i++)
            {
                var block = MakeBlock(MakeContent(MakeDefinition(keys, MakeData(keys))));
                var miner = new Miner(i);
                
                miner.Mined += b =>
                {
                    Console.WriteLine("-----------");
                    Console.WriteLine(i);
                    Console.WriteLine("Milliseconds: {0}" ,timer.Elapsed.Milliseconds);
                    Console.WriteLine("Seconds: {0}" ,timer.Elapsed.Seconds);
                    Console.WriteLine("Minutes: {0}" ,timer.Elapsed.Minutes);
                    Console.WriteLine(Hash.ByteArrayToString(b.Hash));
                    Console.WriteLine("MINED");
                    timer.Restart();
                };
                
                await miner.Start(block);
            }
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