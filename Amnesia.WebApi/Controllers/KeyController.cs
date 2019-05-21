using Amnesia.Application.Services;
using Amnesia.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Text;

namespace Amnesia.WebApi.Controllers
{
    [Route("keys/")]
    [ApiController]
    public class KeyController : ControllerBase
    {
        private DefinitionService service;

        public KeyController(DefinitionService service)
        {
            this.service = service;
        }

        [HttpGet("{publicKey}/definitions")]
        public async Task<ActionResult> GetAsync(string publicKey)
        {
            byte[] key = Encoding.ASCII.GetBytes(publicKey);
            var definition = await service.GetLastDefinition(key);

            if (definition == null)
            {
                return NotFound();
            }

            return Ok(new DefinitionViewModel(definition));
        }
    }
}