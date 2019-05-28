using System.Collections.Generic;
using System.Linq;

namespace Amnesia.Application.Validation
{
    public class ValidationResult
    {
        public bool IsSuccess { get; private set; }
        
        public IEnumerable<byte[]> MissingData { get; private set; }

        public bool IsTotalSuccess => IsSuccess && !MissingData.Any();

        public string Message { get; private set; } = string.Empty;

        public static ValidationResult Success()
        {
            return new ValidationResult
            {
                IsSuccess = true,
                MissingData = Enumerable.Empty<byte[]>()
            };
        }

        public static ValidationResult Success(IEnumerable<byte[]> missingData)
        {
            return new ValidationResult
            {
                IsSuccess = true,
                MissingData = missingData
            };
        }

        public static ValidationResult Failure(string message = "")
        {
            return new ValidationResult
            {
                IsSuccess = false,
                MissingData = Enumerable.Empty<byte[]>(),
                Message = message
            };
        }
    }
}