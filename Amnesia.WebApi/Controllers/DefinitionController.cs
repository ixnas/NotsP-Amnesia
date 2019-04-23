using Microsoft.AspNetCore.Mvc;

namespace Amnesia.WebApi.Controllers
{
    [ApiController]
    public class DefinitionController: ControllerBase
    {
        [HttpGet("definition/block")]
        public ActionResult<string> Block()
        {

            return "Hallo";
        }
        
    }
}