using Microsoft.AspNetCore.Mvc;

namespace Acreator.Controllers
{
    public class HomeController : ControllerBase
    {
        // GET
        public IActionResult Index()
        {
            return Ok();
        }
    }
}