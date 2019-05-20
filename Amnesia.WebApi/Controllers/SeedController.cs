using Amnesia.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Amnesia.WebApi.Controllers
{
    [Route("seed/")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private SeedService seedService;

        public SeedController(SeedService seedService)
        {
            this.seedService = seedService;
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            seedService.SeedData();
            return Ok("init data zit in de db");
        }
    }
}