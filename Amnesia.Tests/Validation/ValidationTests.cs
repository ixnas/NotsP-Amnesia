using System.Linq;
using Amnesia.Application.Validation;
using Amnesia.Application.Validation.Context;
using Amnesia.Application.Validation.Result;
using Amnesia.Cryptography;
using Amnesia.Domain.Model;
using NUnit.Framework;

namespace Amnesia.Tests.Validation
{
    public class ValidationTests
    {
        private MemoryValidationContext context;
        private byte[] block;

        [SetUp]
        public void Setup()
        {
            (context, block) = new TestData().ConstructTestData();
        }
        
        [Test]
        public void ShouldValidateCorrectChain()
        {
            var validator = new BlockValidator(context, 0);
            
            var result = validator.ValidateBlock(block);
            
            LogMessage(result);
            Assert.IsInstanceOf<BlockSuccessResult>(result);
        }

        private static void LogMessage(IBlockValidationResult result)
        {
            if (result is BlockFailureResult failure)
                TestContext.WriteLine(failure.Message);
        }

        [Test]
        public void ShouldInvalidateIncorrectProofOfWork()
        {
            var validator = new BlockValidator(context, 30);

            var result = validator.ValidateBlock(block);

            LogMessage(result);
            Assert.IsInstanceOf<BlockFailureResult>(result);
        }
        
        [Test]
        public void ShouldInvalidateNotFoundBlocks()
        {
            context.GetBlockAndContent(block).PreviousBlockHash = Hash.StringToByteArray("abcde0");
            
            var validator = new BlockValidator(context, 0);

            var result = validator.ValidateBlock(block);

            LogMessage(result);
            Assert.IsInstanceOf<BlockFailureResult>(result);
        }
        
        [Test]
        public void ShouldInvalidateWrongHash()
        {
            context.GetBlockAndContent(block).Hash = Hash.StringToByteArray("abcde0");
            
            var validator = new BlockValidator(context, 0);

            var result = validator.ValidateBlock(block);

            LogMessage(result);
            Assert.IsInstanceOf<BlockFailureResult>(result);
        }
        
        [Test]
        public void ShouldInvalidateWrongContentHash()
        {
            context.GetBlockAndContent(block).Content.Mutations[0] = Hash.StringToByteArray("abcde0");
            
            var validator = new BlockValidator(context, 0);

            var result = validator.ValidateBlock(block);

            LogMessage(result);
            Assert.IsInstanceOf<BlockFailureResult>(result);
        }
        
        [Test]
        public void ShouldInvalidateMissingDefinition()
        {
            context.Definitions.Remove(context.Definitions.Keys.First());
            
            var validator = new BlockValidator(context, 0);

            var result = validator.ValidateBlock(block);

            LogMessage(result);
            Assert.IsInstanceOf<BlockFailureResult>(result);
        }
        
        [Test]
        public void ShouldInvalidateWrongSignature()
        {
            var def = context.Definitions.Values.First();
            def.IsMutable = !def.IsMutable;

            var validator = new BlockValidator(context, 0);

            var result = validator.ValidateBlock(block);

            LogMessage(result);
            Assert.IsInstanceOf<BlockFailureResult>(result);
        }

        [Test]
        public void ShouldInvalidateWrongPreviousDefinition()
        {
            var first = context.Definitions.Values.First();

            var newDefinition = TestData.CreateDefinition("This definition is malicious", TestData.Keys, first.Hash);
            context.AddDefinition(newDefinition);
            context.AddData(newDefinition.Data);

            var newBlock = TestData.CreateBlock(newDefinition, context.GetBlockAndContent(block));
            context.AddBlock(newBlock);
            context.AddContent(newBlock.Content);

            var validator = new BlockValidator(context, 0);

            var result = validator.ValidateBlock(newBlock.Hash);

            LogMessage(result);
            Assert.IsInstanceOf<BlockFailureResult>(result);
        }

        [Test]
        public void ShouldInvalidateMutationsThatReferToADefinitionThatIsNotMutable()
        {
            var last = context.Definitions.Values.Last();

            var mutation = TestData.CreateMutation(last.Hash, TestData.Keys, last.Hash);
            context.AddDefinition(mutation);
            context.AddData(mutation.Data);

            var newBlock = TestData.CreateBlock(mutation, context.GetBlockAndContent(block));
            context.AddBlock(newBlock);
            context.AddContent(newBlock.Content);

            var validator = new BlockValidator(context, 0);

            var result = validator.ValidateBlock(newBlock.Hash);

            LogMessage(result);
            Assert.IsInstanceOf<BlockFailureResult>(result);
        }

        [Test]
        public void ShouldInvalidateMutationsThatReferToADefinitionByADifferentKey()
        {
            var first = context.Definitions.Values.First();

            var keys = new KeyPair(2048);

            var mutation = TestData.CreateMutation(first.Hash, keys, null);
            context.AddDefinition(mutation);
            context.AddData(mutation.Data);

            var newBlock = TestData.CreateBlock(mutation, context.GetBlockAndContent(block));
            context.AddBlock(newBlock);
            context.AddContent(newBlock.Content);

            var validator = new BlockValidator(context, 0);

            var result = validator.ValidateBlock(newBlock.Hash);

            LogMessage(result);
            Assert.IsInstanceOf<BlockFailureResult>(result);
        }

        [Test]
        public void ShouldReturnMissingData()
        {
            var first = context.Definitions.Values.First();
            context.Data.Remove(first.DataHash);
            first.Data = null;

            var validator = new BlockValidator(context, 0);

            var result = validator.ValidateBlock(block);

            LogMessage(result);
            Assert.IsInstanceOf<BlockAcceptableResult>(result);
        }

        [Test]
        public void ShouldNotReturnMissingDataThatHasAnExcuse()
        {
            var definition = context.Definitions.Values.ToList()[2];
            context.Data.Remove(definition.DataHash);
            definition.Data = null;

            var validator = new BlockValidator(context, 0);

            var result = validator.ValidateBlock(block);

            LogMessage(result);
            Assert.IsInstanceOf<BlockSuccessResult>(result);
        }
    }
}