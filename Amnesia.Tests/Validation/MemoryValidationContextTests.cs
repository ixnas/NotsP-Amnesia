using System.Linq;
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

            Assert.That(defs, Has.Count.EqualTo(3));
        }
    }
}