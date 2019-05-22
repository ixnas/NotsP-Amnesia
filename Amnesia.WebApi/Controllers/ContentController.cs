using System;
using System.IO;
using System.Threading.Tasks;
using Amnesia.Application.Services;
using Amnesia.Domain.Model;
using Amnesia.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Amnesia.WebApi.Controllers
{
    [Route("contents/")]
    [ApiController]
    public class ContentController : ControllerBase
    {
        private ContentService service;
        public ContentController(ContentService service)
        {
            this.service = service;
        }
        
        [HttpGet("{hash}")]
        public async Task<IActionResult> Get(string hash)
        {
            var content = await service.GetContent(Hash.StringToByteArray(hash));
            return Ok(ContentViewModel.FromContent(content));
        }
    }
}