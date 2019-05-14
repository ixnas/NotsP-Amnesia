using System;
using System.Threading;
using System.Threading.Tasks;
using Amnesia.Application.Mining;
using Amnesia.Domain.Entity;
using NUnit.Framework;

namespace Amnesia.Tests.Mining
{
    [TestFixture]
    public class MiningTest
    {
        [Test]
        public void TestMiningWithDifficultyOfTen()
        {
            var miner = new Miner(10);
            var block = new Block();
            miner.Mined += b =>
            {
                Assert.AreEqual(b.HashObject(), block.HashObject());
                Assert.AreEqual(b.Nonce, block.Nonce);
            };
            
             miner.Start(block);
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
    }
}