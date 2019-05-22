using System;
using System.Threading.Tasks;
using Amnesia.Application.Services;
using Amnesia.Domain.Model;
using Amnesia.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Amnesia.WebApi.Controllers
{
    [Route("definitions/")]
    [ApiController]
    public class DefinitionController: ControllerBase
    {
        private DefinitionService service;

        public DefinitionController(DefinitionService service)
        {
            this.service = service;
        }
        
        [HttpGet("{hash}")]
        public async Task<IActionResult> Get(string hash)
        {
            var definition = await service.GetDefinition(Hash.StringToByteArray(hash));
            return Ok(DefinitionViewModel.FromDefinition(definition));
        }
        
        [HttpPost]
        public void Post([FromBody] string value)
        {
            var deserialized = JsonConvert.DeserializeObject(value);
            Console.WriteLine(deserialized);
            //TODO: send to chain
        }
    }
}