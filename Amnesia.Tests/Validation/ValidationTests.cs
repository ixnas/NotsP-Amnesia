using System;
using System.Linq;
using System.Text;
using Amnesia.Application.Validation;
using Amnesia.Application.Validation.Context;
using Amnesia.Domain.Entity;
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
            
            TestContext.WriteLine(result.Message);
            Assert.True(result.IsTotalSuccess);
        }

        [Test]
        public void ShouldInvalidateIncorrectProofOfWork()
        {
            var validator = new BlockValidator(context, 30);

            var result = validator.ValidateBlock(block);
            
            TestContext.WriteLine(result.Message);
            Assert.False(result.IsTotalSuccess);
        }
        
        [Test]
        public void ShouldInvalidateNotFoundBlocks()
        {
            context.GetBlockAndContent(block).PreviousBlockHash = Hash.StringToByteArray("abcde0");
            
            var validator = new BlockValidator(context, 0);

            var result = validator.ValidateBlock(block);
            
            TestContext.WriteLine(result.Message);
            Assert.False(result.IsTotalSuccess);
        }
        
        [Test]
        public void ShouldInvalidateWrongHash()
        {
            context.GetBlockAndContent(block).Hash = Hash.StringToByteArray("abcde0");
            
            var validator = new BlockValidator(context, 0);

            var result = validator.ValidateBlock(block);
            
            TestContext.WriteLine(result.Message);
            Assert.False(result.IsTotalSuccess);
        }
        
        [Test]
        public void ShouldInvalidateWrongContentHash()
        {
            context.GetBlockAndContent(block).Content.Definitions[0] = Hash.StringToByteArray("abcde0");
            
            var validator = new BlockValidator(context, 0);

            var result = validator.ValidateBlock(block);
            
            TestContext.WriteLine(result.Message);
            Assert.False(result.IsTotalSuccess);
        }
        
        [Test]
        public void ShouldInvalidateMissingDefinition()
        {
            context.Definitions.Remove(context.Definitions.Keys.First());
            
            var validator = new BlockValidator(context, 0);

            var result = validator.ValidateBlock(block);
            
            TestContext.WriteLine(result.Message);
            Assert.False(result.IsTotalSuccess);
        }
        
        [Test]
        public void ShouldInvalidateWrongSignature()
        {
            var def = context.Definitions.Values.First();
            def.IsMutable = !def.IsMutable;

            var validator = new BlockValidator(context, 0);

            var result = validator.ValidateBlock(block);
            
            TestContext.WriteLine(result.Message);
            Assert.False(result.IsTotalSuccess);
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

            TestContext.WriteLine(result.Message);
            Assert.False(result.IsTotalSuccess);
        }
    }
}