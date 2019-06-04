using System.Collections.Generic;

namespace Amnesia.Application.Validation.Result
{
    public interface IBlockValidationResult
    {
    }

    public class BlockSuccessResult : IBlockValidationResult { }

    public class BlockFailureResult : IBlockValidationResult
    {
        public string Message { get; }

        public BlockFailureResult(string message)
        {
            Message = message;
        }
    }

    public class BlockAcceptableResult : IBlockValidationResult
    {
        public IEnumerable<byte[]> MissingData { get; }

        public BlockAcceptableResult(IEnumerable<byte[]> missingData)
        {
            MissingData = missingData;
        }
    }
}