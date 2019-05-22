using System.Collections.Generic;

namespace Amnesia.Application.Validation
{
    public interface IValidationResult
    {
        /// <summary>
        /// If the validation has succeeded and no missing data was found
        /// </summary>
        bool IsSuccess { get; }

        /// <summary>
        /// If the validation can continue
        /// </summary>
        bool IsAcceptable { get; }
    }

    public class SuccessResult : IValidationResult
    {
        public bool IsSuccess => true;
        public bool IsAcceptable => true;
    }

    public class FailureResult : IValidationResult
    {
        public bool IsSuccess => false;
        public bool IsAcceptable => false;
    }

    public class MissingDataResult : IValidationResult
    {
        public IList<byte[]> MissingData { get; set; } = new List<byte[]>();

        public bool IsSuccess { get; }
        public bool IsAcceptable { get; }
    }

    public static class Result
    {
        public static FailureResult Failure()
        {
            return new FailureResult();
        }

        public static SuccessResult Success()
        {
            return new SuccessResult();
        }
    }
}