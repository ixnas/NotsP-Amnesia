using System;
using System.Linq;
using System.Threading.Tasks;
using Amnesia.Application.Peers;
using Amnesia.Application.Services;
using Amnesia.Domain.Model;
using Amnesia.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Amnesia.WebApi.Controllers
{
    [Route("blocks")]
    [ApiController]
    public class BlockController : ControllerBase
    {
        private readonly BlockService service;
        private readonly PeerManager manager;
        private readonly Application.Amnesia amnesia;

        public BlockController(BlockService service, PeerManager manager, Application.Amnesia amnesia)
        {
            this.service = service;
            this.manager = manager;
            this.amnesia = amnesia;
        }

        [HttpGet("{hash}")]
        public IActionResult Get(string hash)
        {
            var block = service.GetBlock(Hash.StringToByteArray(hash));
            return Ok(BlockViewModel.FromBlock(block));
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            var blocks = service.GetBlocks();
            var list = blocks.Select(b => BlockViewModel.FromBlock(b));
            return Ok(list);
        }
        
//         TODO: Refactor function and viewmodel
//        [HttpGet]
//        public async Task<ActionResult> GetWithDepth([FromQuery(Name = "depth")] int depth)
//        {
//            var blocks = await service.GetBlocks(depth);
//            return Ok(new BlockchainViewModel(blocks));
//        }

        [HttpPost]
        public async Task<IActionResult> Post([FromQuery(Name = "peer")] string peer, [FromBody] string value)
        {    
            Console.WriteLine(value);
            await amnesia.ReceiveBlock(Hash.StringToByteArray(value), peer);
            return Ok();
        }
    }
}