using System.Threading.Tasks;
using Acreator.Models;
using Acreator.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Acreator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepo _repo;
        public AuthController(IAuthRepo repo)
        {
            this._repo = repo;
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm]Admin creds)
        {
            var repoResponse = await _repo.Login(creds);
            Response.StatusCode = repoResponse.StatusCode;

            if (!repoResponse.IsSuccess)
            {
                return new JsonResult(new { error_code = repoResponse.StatusCode, status = 1});
            }

            return new JsonResult(new { data = repoResponse.Content, status = 0 });
        }
    }
}