using System;
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
        private readonly DefinitionService service;
        private readonly Application.Amnesia amnesia;

        public DefinitionController(DefinitionService service, Application.Amnesia amnesia)
        {
            this.service = service;
            this.amnesia = amnesia;
        }
        
        [HttpGet("{hash}")]
        public async Task<IActionResult> Get(string hash)
        {
            var definition = await service.GetDefinition(Hash.StringToByteArray(hash));

            if (definition == null)
            {
                return NotFound();
            }
            return Ok(DefinitionViewModel.FromDefinition(definition));
        }

        /// <summary>
        /// Gets a definition by key.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("last")]
        public async Task<ActionResult> GetLastByKey([FromBody] GetByKeyModel model)
        {
            var definition = await service.GetLastDefinition(model.PublicKey);

            if (definition == null)
            {
                return NotFound();
            }

            return Ok(DefinitionViewModel.FromDefinition(definition));
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
                PreviousDefinitionHash  = model.Definition.PreviousDefinitionHash == null ? null : Hash.StringToByteArray(model.Definition.PreviousDefinitionHash),
                Signature               = Convert.FromBase64String(model.Definition.Data.Signature),
                Blob                    = Convert.FromBase64String(model.Definition.Data.Blob),
                Key                     = model.Key
            };

            var definition = new Definition
            {
                DataHash                = Hash.StringToByteArray(model.Definition.DataHash),
                PreviousDefinitionHash  = model.Definition.PreviousDefinitionHash == null ? null : Hash.StringToByteArray(model.Definition.PreviousDefinitionHash),
                Signature               = Convert.FromBase64String(model.Definition.Signature),
                Key                     = model.Key,
                IsMutation              = model.Definition.IsMutation,
                IsMutable               = model.Definition.IsMutable,
                Data                    = data,
                PreviousDefinition      = null
            };

            await amnesia.ReceiveDefinition(definition);

            return Ok("Node gemined");
        }
    }
}