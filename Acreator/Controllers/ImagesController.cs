using System.Threading.Tasks;
using Acreator.Models;
using Acreator.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Acreator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ImagesController : ControllerBase
    {
        private readonly IImagesRepo _repo;

        public ImagesController(IImagesRepo repo)
        {
            this._repo = repo;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var repoResponse = await _repo.GetAll();

            Response.StatusCode = repoResponse.StatusCode;
            if (!repoResponse.IsSuccess)
            {
                return new JsonResult(new { error_code = repoResponse.StatusCode, status = 1 });
            }

            return new JsonResult(new { data = repoResponse.Content, status = 0 });
        }

        [HttpGet("filter/{purpose}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFiltered(ImagePurpose purpose)
        {
            var repoResponse = await _repo.GetFiltered(purpose);

            Response.StatusCode = repoResponse.StatusCode;
            if (!repoResponse.IsSuccess)
            {
                return new JsonResult(new { error_code = repoResponse.StatusCode, status = 1 });
            }

            return new JsonResult(new { data = repoResponse.Content, status = 0 });
        }

        [HttpPost("new")]
        public async Task<IActionResult> AddImage([FromForm] ImageAddDto newImage)
        {
            var repoResponse = await _repo.AddImage(newImage);

            Response.StatusCode = repoResponse.StatusCode;
            if (!repoResponse.IsSuccess)
            {
                return new JsonResult(new { error_code = repoResponse.StatusCode, status = 1 });
            }

            return new JsonResult(new { data = repoResponse.Content, status = 0 });
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var repoResponse = await _repo.DeleteImage(id);

            Response.StatusCode = repoResponse.StatusCode;
            if (!repoResponse.IsSuccess)
            {
                return new JsonResult(new { error_code = repoResponse.StatusCode, status = 1 });
            }

            return new JsonResult(new { data = repoResponse.Content, status = 0 });
        }
    }
}