using System;
using System.Threading.Tasks;
using Amnesia.Application.Services;
using Amnesia.Domain.Entity;
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
        public async Task<ActionResult> Get(string hash)
        {
            var definition = await service.GetDefinition(new Hash(hash).Bytes);
            return Ok(new DefinitionViewModel(definition));
        }
        
        [HttpGet("{hash}/data")]
        public async Task<ActionResult> GetData(string hash)
        {
            var content = await service.GetDefinition(new Hash(hash).Bytes, true);
            return Ok(content.Data.Blob);
        }

        /// <summary>
        /// Gets the latest definition that was added to the chain. This is an API request for the client to get a PreviousHashDefinition.
        /// </summary>
        /// <returns></returns>
        [HttpGet("last")]
        public async Task<ActionResult> GetLastDefinition()
        {
            var definition = await service.GetLastDefinition();

            if (definition == null)
            {
                return NotFound();
            }

            return Ok(new DefinitionViewModel(definition));
        }

        /// <summary>
        /// Creates a new definition to add to the chain. Receives a definition signed by the client via a POST request.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> CreateDefinition([FromBody] string value)
        {
            var deserialized = JsonConvert.DeserializeObject(value);
            Console.WriteLine(deserialized);
            //var definition = new Definition();
            //await service.AddDefinition(definition);

            return Ok();
        }
    }
}