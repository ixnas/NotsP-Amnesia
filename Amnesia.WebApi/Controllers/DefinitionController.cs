using System;
using System.Linq;
using System.Threading.Tasks;
using Amnesia.Application.Services;
using Amnesia.Domain.Entity;
using Amnesia.Domain.Model;
using Amnesia.Domain.ViewModels;
using Amnesia.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Amnesia.WebApi.Controllers
{
    [Route("definitions")]
    [ApiController]
    public class DefinitionController: ControllerBase
    {
        private readonly Application.Amnesia amnesia;
        private readonly BlockchainService blockchain;
        private readonly StateService stateService;

        public DefinitionController(Application.Amnesia amnesia, BlockchainService blockchain, StateService stateService)
        {
            this.amnesia = amnesia;
            this.blockchain = blockchain;
            this.stateService = stateService;
        }
        
        [HttpGet("{hash}")]
        public IActionResult Get(string hash)
        {
            var definition = blockchain.ValidationContext.GetDefinition(Hash.StringToByteArray(hash));

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
        public ActionResult GetLastByKey([FromBody] GetByKeyModel model)
        {
            var definitionHash = blockchain.ValidationContext
                .GetDefinitionsByKey(model.PublicKey, stateService.State.CurrentBlockHash)
                .FirstOrDefault();

            if (definitionHash == null)
            {
                return NotFound();
            }

            var definition = blockchain.ValidationContext.GetDefinition(definitionHash);

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
            };

            await amnesia.ReceiveDefinition(definition);

            return Ok("Nieuw block gemined");
        }

        [HttpGet("{hash}/data")]
        public IActionResult GetData(string hash)
        {
            var data = blockchain.ValidationContext.GetData(Hash.StringToByteArray(hash));

            if (data == null)
            {
                return NotFound("The data may have been deleted");
            }

            return Ok(DataViewModel.FromData(data));
        }

        [HttpGet("{hash}/data/blob")]
        public IActionResult GetDataBlob(string hash)
        {
            var data = blockchain.ValidationContext.GetData(Hash.StringToByteArray(hash));

            if (data == null)
            {
                return NotFound("The data may have been deleted");
            }

            return Ok(data.Blob);
        }
    }
}