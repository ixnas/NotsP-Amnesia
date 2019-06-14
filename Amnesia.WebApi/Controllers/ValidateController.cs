using Amnesia.Application.Services;
using Amnesia.Application.Validation;
using Microsoft.AspNetCore.Mvc;

namespace Amnesia.WebApi.Controllers
{
    public class ValidateController : Controller
    {
        private readonly BlockchainService blockchain;
        private readonly StateService stateService;

        public ValidateController(BlockchainService blockchain, StateService stateService)
        {
            this.blockchain = blockchain;
            this.stateService = stateService;
        }

        [HttpGet]
        public IActionResult Validate()
        {
            var state = stateService.State.CurrentBlockHash;
            var context = blockchain.ValidationContext;

            var validator = new BlockValidator(context, 20);
            validator.UseAssumptions = false;

            var result = validator.ValidateBlock(state);

            return Ok(result.Message);
        }
    }
}