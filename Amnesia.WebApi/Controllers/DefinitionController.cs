using System;
using System.Text;
using System.Threading.Tasks;
using Amnesia.Application.Services;
using Amnesia.Domain.Entity;
using Amnesia.Domain.Model;
using Amnesia.Domain.ViewModels;
using Amnesia.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

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
            byte[] key = Encoding.ASCII.GetBytes(model.PublicKey);
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
        public async Task<ActionResult> CreateDefinition([FromBody] AddDefinitionModel model)
        {
            var data = new Data
            {
                PreviousDefinitionHash  = Encoding.ASCII.GetBytes(model.Definition.PreviousDefinitionHash),
                Signature               = Convert.FromBase64String(model.Definition.Data.Signature),
                Blob                    = Encoding.ASCII.GetBytes(model.Definition.Data.Blob)
            };

            var definition = new Definition
            {
                DataHash                = Encoding.ASCII.GetBytes(model.Definition.Hash),
                PreviousDefinitionHash  = Encoding.ASCII.GetBytes((model.Definition.PreviousDefinitionHash)),
                Signature               = Convert.FromBase64String(model.Definition.Signature),
                Key                     = Encoding.ASCII.GetBytes(model.Key),
                IsMutation              = model.Definition.Meta.isMutation,
                IsMutable               = model.Definition.Meta.isMutable,
                Data                    = data,
                PreviousDefinition      = null
            };

            //TODO: Node laten minen naar block

            return Ok();
        }
    }
}