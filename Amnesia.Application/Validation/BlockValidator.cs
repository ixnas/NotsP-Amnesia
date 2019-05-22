using Amnesia.Application.Validation.Context;

namespace Amnesia.Application.Validation
{
    public class BlockValidator
    {
        private readonly IValidationContext context;

        public BlockValidator(IValidationContext context)
        {
            this.context = context;
        }

        public IValidationResult ValidateBlock(byte[] hash)
        {
            if (context.ShouldAssumeValid(hash))
            {
                return Result.Success();
            }

            var block = context.GetBlockAndContent(hash);
            var prevBlockResult = ValidateBlock(block.PreviousBlockHash);

            if (!prevBlockResult.IsAcceptable)
            {
                return Result.Failure();
            }

            

            if (!ValidateProofOfWork(block.Hash))
            {
                return Result.Failure();
            }

            foreach (var definition in block.Content.Definitions)
            {
                
            }


            return Result.Success();
        }

        public bool ValidateProofOfWork(byte[] hash)
        {
            // TODO
            return true;
        }
    }
}