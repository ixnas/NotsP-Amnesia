using System;
using Amnesia.Application.Peers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Amnesia.WebApi.Controllers
{
    public class KeyController : ControllerBase

    {
        private PeerManager _manager;
    
        public KeyController(PeerManager manager)
        {
            _manager = manager;
        }
    
        [HttpGet("/keys/{publicKey}/definitions")]
        public ActionResult Get(string publicKey, [FromQuery(Name = "limit")] int limit)
        {
            if (limit < 1 || limit > 100)
            {
                return Conflict("Limit must be between 1 and 100.");
            }
    
            return Ok(_manager.GetDefinitions(publicKey, limit));
        }
    }
}