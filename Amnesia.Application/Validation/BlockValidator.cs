using System;

namespace Amnesia.Application.Validation
{
    public class BlockValidator
    {
        public bool ValidateBlock(byte[] hash, MemoryValidationContext context)
        {
            var block = context.GetBlockAndContent(hash);

            throw new NotImplementedException();
        }
    }
}