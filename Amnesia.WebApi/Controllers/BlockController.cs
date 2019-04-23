using Amnesia.Domain.Entity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Amnesia.WebApi.Controllers
{
    [Route("blocks/")]
    [ApiController]
    public class BlockController
    {
        [HttpGet("{hash}")]
        public ActionResult<string> Get(string hash)
        {
            //TODO get block where hash = hash from entity service layer
            var block = new Block {Nonce = 1};
            var json = JsonConvert.SerializeObject(block);
            return json;
        }

        [HttpGet("?depth={depth}")]
        public ActionResult<string> Get(int depth)
        {
            //TODO get blocks

            
            
            var json = JsonConvert.SerializeObject();

            return json;
        }
    }
}