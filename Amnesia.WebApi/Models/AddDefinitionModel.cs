using Amnesia.Domain.ViewModels;

namespace Amnesia.WebApi.Models
{
    public class AddDefinitionModel
    {
        public DefinitionViewModel Definition { get; set; }
        public AddDataViewModel Data { get; set; }
    }
}
