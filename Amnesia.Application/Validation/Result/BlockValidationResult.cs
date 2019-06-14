using System.Collections.Generic;
using System.Linq;
using Amnesia.Domain.Model;

namespace Amnesia.Application.Validation.Result
{
    public interface IBlockValidationResult
    {
        string Message { get; }
    }

    public class BlockSuccessResult : IBlockValidationResult
    {
        public string Message => "Success";
    }

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

        public string Message => "Missing Data: " + string.Join(", ", MissingData.Select(Hash.ByteArrayToString));


    }
}