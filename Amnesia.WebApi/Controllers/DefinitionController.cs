using System;
using System.Text;
using System.Threading.Tasks;
using Amnesia.Application.Services;
using Amnesia.Domain.Model;
using Amnesia.Domain.ViewModels;
using Amnesia.WebApi.Models;
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

            if (definition == null)
            {
                return NotFound();
            }

            return Ok(new DefinitionViewModel(definition));
        }

        /// <summary>
        /// Gets a definition by key.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("last")]
        public async Task<ActionResult> GetByKey([FromBody] GetByKeyModel model)
        {
            byte[] key = Encoding.ASCII.GetBytes(model.publicKey);
            var definition = await service.GetLastDefinition(); //REMINDER: add in key after debugging (to search for definition by key)

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