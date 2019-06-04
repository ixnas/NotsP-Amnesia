namespace Amnesia.Application.Validation.Result
{
    public interface IDefinitionValidationResult
    {
    }

    public class DefinitionSuccessResult : IDefinitionValidationResult { }

    public class DefinitionMissingDataResult : IDefinitionValidationResult { }

    public class DefinitionDeletedDataResult : IDefinitionValidationResult
    {
        public byte[] ReferencingDefinition { get; }

        public DefinitionDeletedDataResult(byte[] referencingDefinition)
        {
            ReferencingDefinition = referencingDefinition;
        }
    }

    public class DefinitionFailureResult : IDefinitionValidationResult
    {
        public string Message { get; }

        public DefinitionFailureResult(string message)
        {
            Message = message;
        }
    }
}