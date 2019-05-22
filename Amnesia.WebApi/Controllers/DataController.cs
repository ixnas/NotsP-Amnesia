using System.Text;
using System.Threading.Tasks;
using Amnesia.Application.Services;
using Amnesia.Domain.Model;
using Amnesia.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Amnesia.WebApi.Controllers
{
    [Route("data/")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private DataService service;

        public DataController(DataService dataService)
        {
            this.service = dataService;
        }
        
        [HttpGet("{hash}")]
        public async Task<IActionResult> Get(string hash)
        {
            var data = await service.GetData(Hash.StringToByteArray(hash));
            return Ok(DataViewModel.FromData(data));
        }
        
        [HttpGet("{hash}/data")]
        public async Task<IActionResult> GetBlob(string hash)
        {
            var data = await service.GetData(Hash.StringToByteArray(hash));
            return Ok(Encoding.ASCII.GetString(data.Blob));
        }
    }
}