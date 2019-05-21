using System;

namespace Amnesia.Application.Validation
{
    public class BlockValidator
    {
        private readonly MemoryValidationContext memory;
        private readonly DatabaseValidationContext database;

        public BlockValidator(MemoryValidationContext memory, DatabaseValidationContext database)
        {
            this.memory = memory;
            this.database = database;
        }

        public bool ValidateBlock(byte[] hash)
        {
            var block = memory.GetBlockAndContent(hash);

            throw new NotImplementedException();
        }
    }
}