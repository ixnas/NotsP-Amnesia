using System.Threading.Tasks;
using Amnesia.Application.Peers;
using Amnesia.Application.Services;
using Amnesia.Domain.Model;
using Amnesia.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Amnesia.WebApi.Controllers
{
    [Route("blocks/")]
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
        public async Task<ActionResult> Get(string hash)
        {
            var block = await service.GetBlock(new Hash(hash).Bytes);
            return Ok(new BlockViewModel(block));
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery(Name = "depth")] int depth)
        {
            var blocks = await service.GetBlocks(depth);
            return Ok(new BlockchainViewModel(blocks));
        }

        [HttpPost]
        public async Task Post([FromQuery(Name = "id")] string id, [FromBody] string value)
        {
            var block = await service.GetBlock(new Hash(value).Bytes);
            var peer = manager.GetPeer(id);
            //amnesia.ReceiveBlock(block, peer);
        }
    }
}