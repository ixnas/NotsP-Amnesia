using System;
using System.Text;
using Amnesia.Application.Peers;
using Amnesia.Application.Services;
using Amnesia.Domain.Context;
using Amnesia.Domain.Entity;
using Amnesia.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Amnesia.WebApi.Controllers
{
    [Route("blocks/")]
    [ApiController]
    public class BlockController: IController
    {
        private readonly BlockService _service;
        private readonly PeerManager _manager;

        public BlockController(BlockService service, PeerManager manager)
        {
            _service = service;
            _manager = manager;
        }
         
        [HttpGet("{hash}")]
        public ActionResult<string> Get(string hash)
        {
            try
            {
                var bytes = new Hash(hash).Bytes;
                var block = _service.GetBlock(bytes);
                var json = JsonConvert.SerializeObject(block);
                return json;
            }
            catch (Exception error)
            {
                Console.WriteLine("{0} : {1}", error.Message, error.StackTrace);
                return error.Message;
            }
        }

        [HttpGet]
        public ActionResult<string> Get([FromQueryAttribute(Name = "depth")] int depth)
        {
            try
            {
                var blocks = _service.GetBlocks(depth).Result;
                var json = JsonConvert.SerializeObject(blocks);
                return json;
            }
            catch (Exception error)
            {
                Console.WriteLine("{0} : {1}", error.Message, error.StackTrace);
                return error.Message;
            }
        }

        [HttpPost]
        public void Post([FromQueryAttribute(Name = "id")] string id, [FromBody] string value)
        {
            try
            {
                _manager.Inform(id, value);
            }
            catch (Exception error)
            {
                Console.WriteLine("{0} : {1}", error.Message, error.StackTrace);
            }
        }
    }
}