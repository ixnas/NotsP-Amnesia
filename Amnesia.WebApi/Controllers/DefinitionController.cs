using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amnesia.Application.Services;
using Amnesia.Cryptography;
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
                .GetDefinitionsByKey(model.Key, stateService.State.CurrentBlockHash)
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
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateDefinition([FromBody] AddDefinitionModel model)
        {
            
            var data = new Data
            {
                PreviousDefinitionHash  = model.Data.PreviousDefinition == null ? null : Hash.StringToByteArray(model.Data.PreviousDefinition),
                Signature               = model.Data.Signature,
                Blob                    = model.Data.Blob,
                Key                     = model.Data.Key
            };

            var definition = new Definition
            {
                DataHash                = Hash.StringToByteArray(model.Definition.DataHash),
                PreviousDefinitionHash  = model.Definition.PreviousDefinition == null ? null : Hash.StringToByteArray(model.Definition.PreviousDefinition),
                Signature               = model.Definition.Signature,
                Key                     = model.Definition.Key,
                IsMutation              = model.Definition.IsMutation,
                IsMutable               = model.Definition.IsMutable,
                Data                    = data,
            };

            data.Hash = data.HashObject();
            definition.DataHash = data.Hash;
            definition.Hash = definition.HashObject();   
            await amnesia.ReceiveDefinition(definition);

            return Ok("Nieuw block gemined");
        }

        [HttpPost("easy")]
        public async Task<IActionResult> CreateDefinitionTheEasyWay([FromBody] EasyDefinitionModel model)
        {
            var previousDefinition = blockchain.ValidationContext
                .GetDefinitionsByKey(model.PublicKey, stateService.State.CurrentBlockHash)
                .FirstOrDefault();

            var isMutation = !string.IsNullOrEmpty(model.DefinitionHash);
            var data = new Data
            {
                Blob = model.Blob,
                PreviousDefinitionHash = previousDefinition,
                Key = model.PublicKey
            };

            if (isMutation)
            {
                data.Blob = Encoding.UTF8.GetBytes("DELETE " + model.DefinitionHash);
            }
            
            var privateKey = new PrivateKey(model.PrivateKey);

            data.Signature = privateKey.SignData(data.SignatureHash.EncodeToBytes());
            data.Hash = data.HashObject();

            var definition = new Definition
            {
                DataHash = data.Hash,
                Data = data,
                IsMutable = !isMutation,
                IsMutation = isMutation,
                Key = model.PublicKey,
                PreviousDefinitionHash = previousDefinition
            };

            definition.Signature = privateKey.SignData(definition.SignatureHash.EncodeToBytes());
            definition.Hash = definition.HashObject();

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

            return Ok(data.Blob == null ? DataViewModel.FromData(data) : DataViewModel.FromData(data, true));
        }

        [HttpGet("{hash}/data/blob")]
        public async Task<IActionResult> GetDataBlob(string hash)
        {
            var data = blockchain.ValidationContext.GetData(Hash.StringToByteArray(hash));

            if (data == null)
            {
                return NotFound("The data may have been deleted");
            }

            Response.ContentType = "application/octet-stream";
            Response.ContentLength = data.Blob.Length;
            await Response.Body.WriteAsync(data.Blob, 0, data.Blob.Length);
            return Ok();
        }
    }
}