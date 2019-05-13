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
        public async Task TestMiningWithDifficultyOfTen()
        {
            var miner = new Miner(10);
            var block = new Block();
            miner.Mined += b =>
            {
                Assert.AreEqual(b.HashObject(), block.HashObject());
                Assert.AreEqual(b.Nonce, block.Nonce);
            };
            
            await miner.Start(block);
        }

        [Test]
        public void TestStopMining()
        {
            var miner = new Miner(30);
            var block = new Block();
            miner.Start(block);
            miner.Stop();
            Assert.True(miner.cancellationTokenSource.IsCancellationRequested);
        }
    }
}