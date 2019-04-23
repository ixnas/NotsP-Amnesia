using Microsoft.AspNetCore.Mvc;

namespace Amnesia.WebApi.Controllers
{
    public interface IController
    {
        ActionResult<string> Get(string hash);
    }
}