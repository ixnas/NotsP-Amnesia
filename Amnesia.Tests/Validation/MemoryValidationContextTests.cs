using System.Collections.Generic;
using System.Linq;
using Amnesia.Application.Helper;
using Amnesia.Application.Validation.Context;
using Amnesia.Domain.Model;
using NUnit.Framework;

namespace Amnesia.Tests.Validation
{
    public class MemoryValidationContextTests
    {
        [Test]
        public void ShouldFindAChainOfDefinitionsByKey()
        {
            var (context, block) = new TestData().ConstructTestData();

            var key = context.Definitions.Values.First().Key;

            var defs = context.GetDefinitionsByKey(key, block).ToList();

            foreach (var def in defs)
            {
                TestContext.WriteLine(Hash.ByteArrayToString(def));
            }

            Assert.That(defs, Has.Count.EqualTo(4));
        }

        [Test]
        public void ShouldMakeCorrectBlockGraph()
        {
            var (context, block) = new TestData().ConstructTestData();

            var blocks = context.GetBlockGraph(block).ToList();

            foreach (var b in blocks)
            {
                TestContext.WriteLine(Hash.ByteArrayToString(b));
            }

            Assert.That(blocks, Has.Count.EqualTo(4));
            Assert.That(blocks, Does.Not.Contain(null));
            Assert.AreEqual(block, blocks[0]);
        }

        [Test]
        public void CombinedContextShouldMakeIdenticalBlockGraph()
        {
            var (memory, block) = new TestData().ConstructTestData();
            var combined = new CombinedValidationContext { memory };

            var memoryBlocks = memory.GetBlockGraph(block).ToList();
            var combinedBlocks = combined.GetBlockGraph(block).ToList();

            Assert.AreEqual(memoryBlocks, combinedBlocks);
        }

        [Test]
        public void HashesShouldBeUsableAsDictionaryKey()
        {
            var a = Hash.StringToByteArray("abcde0");
            var b = Hash.StringToByteArray("abcde0");
            var c = Hash.StringToByteArray("123456");

            var dict = new Dictionary<byte[], byte[]>(new ByteArrayEqualityComparer())
            {
                {a, a}
            };

            var newA = dict[b];

            Assert.AreSame(a, newA);
            Assert.AreNotEqual(a, c);
            Assert.True(dict.ContainsKey(a));
            Assert.True(dict.ContainsKey(b));

            dict.Remove(b);

            Assert.False(dict.ContainsKey(a));
            Assert.False(dict.ContainsKey(b));
        }
    }
}