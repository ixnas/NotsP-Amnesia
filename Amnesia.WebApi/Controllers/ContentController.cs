using System;
using System.IO;
using Amnesia.Application.Services;
using Amnesia.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Amnesia.WebApi.Controllers
{
    [Route("contents/")]
    [ApiController]
    public class ContentController : IController
    {
        private ContentService _service;

        public ContentController(ContentService service)
        {
            _service = service;
        }

        [HttpGet("hash")]
        public ActionResult<string> Get(string hash)
        {
            try
            {
                var bytes = new Hash(hash).Bytes;
                var content = _service.GetContent(bytes);
                var json = JsonConvert.SerializeObject(content);
                return json;
            }
            catch (Exception error)
            {
                Console.WriteLine("{0} : {1}", error.Message, error.StackTrace);
                return error.Message;
            }
        }
        
        
    }
}