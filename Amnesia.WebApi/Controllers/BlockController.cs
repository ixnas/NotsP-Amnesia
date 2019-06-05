using System;
using System.Linq;
using System.Threading.Tasks;
using Amnesia.Application.Services;
using Amnesia.Domain.Model;
using Amnesia.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Amnesia.WebApi.Controllers
{
    [Route("blocks")]
    [ApiController]
    public class BlockController : ControllerBase
    {
        private readonly Application.Amnesia amnesia;
        private readonly BlockchainService blockchain;
        private readonly StateService stateService;

        public BlockController(Application.Amnesia amnesia, BlockchainService blockchain, StateService stateService)
        {
            this.amnesia = amnesia;
            this.blockchain = blockchain;
            this.stateService = stateService;
        }

        [HttpGet("{hash}")]
        public IActionResult Get(string hash)
        {
            var block = blockchain.ValidationContext.GetBlockAndContent(Hash.StringToByteArray(hash));

            if (block == null)
            {
                return NotFound("The block was not found");
            }

            return Ok(BlockViewModel.FromBlock(block));
        }

        [HttpGet("{hash}/content")]
        public IActionResult GetContent(string hash)
        {
            var block = blockchain.ValidationContext.GetBlockAndContent(Hash.StringToByteArray(hash));

            if (block == null)
            {
                return NotFound("The block was not found");
            }

            return Ok(ContentViewModel.FromContent(block.Content));
        }
        
        [HttpGet]
        public ActionResult GetWithDepth([FromQuery] int? depth)
        {
            if (depth < 1)
            {
                ModelState.AddModelError("depth", "Depth must be greater than 0");
                return BadRequest(ModelState);
            }

            var state = stateService.State.CurrentBlockHash;
            var graph = blockchain.ValidationContext
                .GetBlockGraph(state);

            var blocks = depth.HasValue 
                ? graph.Take(depth.Value).ToList()
                : graph.ToList();

            return Ok(blocks.Select(Hash.ByteArrayToString));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromQuery(Name = "peer")] string peer, [FromBody] string value)
        {    
            Console.WriteLine(value);
            await amnesia.ReceiveBlock(Hash.StringToByteArray(value), peer);
            return Ok();
        }
    }
}