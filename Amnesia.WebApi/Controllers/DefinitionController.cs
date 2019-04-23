using System;
using Amnesia.Application.Services;
using Amnesia.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Amnesia.WebApi.Controllers
{
    [Route("definitions/")]
    [ApiController]
    public class DefinitionController: ControllerBase, IController
    {
        private DefinitionService _service;

        public DefinitionController(DefinitionService service)
        {
            _service = service;
        }
        
        [HttpGet("{hash}")]
        public ActionResult<string> Get(string hash)
        {
            try
            {
                var bytes = new Hash(hash).Bytes;
                var content = _service.GetDefinition(bytes).Result;
                var json = JsonConvert.SerializeObject(content);
                return json;
            }
            catch (Exception error)
            {
                Console.WriteLine("{0} : {1}", error.Message, error.StackTrace);
                return error.Message;
            }
        }
        
        [HttpGet("{hash}/data")]
        public ActionResult<string> GetData(string hash)
        {
            try
            {
                var bytes = new Hash(hash).Bytes;
                var content = _service.GetDefinition(bytes, true).Result;
                var json = JsonConvert.SerializeObject(content);
                return json;
            }
            catch (Exception error)
            {
                Console.WriteLine("{0} : {1}", error.Message, error.StackTrace);
                return error.Message;
            }
        }

        [HttpPost]
        public void Post([FromBody] string value)
        {
            try
            {
                var deserialized = JsonConvert.DeserializeObject(value);
                Console.WriteLine(deserialized);
                //TODO: send to chain
            }
            catch (Exception error)
            {
                Console.WriteLine("{0} : {1}", error.Message, error.StackTrace);
            } 
        }
    }
}