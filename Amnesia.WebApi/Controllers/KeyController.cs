using System;
using Amnesia.Application.Peers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Amnesia.WebApi.Controllers
{
    public class KeyController
    {
        private PeerManager _manager;
        
        public KeyController(PeerManager manager)
        {
            _manager = manager;
        }
        
        [HttpGet("/keys/{publicKey}/definitions")]
        public ActionResult<string> Get(string key, [FromQueryAttribute(Name = "limit")] int limit)
        {
            try
            {
                if (limit < 1 || limit > 100)
                {
                    return "Limit incorrect";
                }

                var json = JsonConvert.SerializeObject(_manager.GetDefinitions(key, limit));
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