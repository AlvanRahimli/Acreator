using System;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Acreator.Models;
using Acreator.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Acreator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "admin")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsRepo _repo;

        public ProductsController(IProductsRepo repo)
        {
            _repo = repo;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<JsonResult> GetProducts()
        {
            var repoResponse = await _repo.GetProducts();

            Response.StatusCode = repoResponse.StatusCode;
            if (repoResponse.IsSuccess)
            {
                return new JsonResult(new { data = repoResponse.Content, status = 0 });
            }

            return new JsonResult(new { error_code = repoResponse.StatusCode, status = 1 });
        }

        [HttpGet("filter/{type}")]
        [AllowAnonymous]
        public async Task<JsonResult> GetFiltered(ProductType type)
        {
            var repoResponse = await _repo.GetFiltered(type);

            Response.StatusCode = repoResponse.StatusCode;
            if (repoResponse.IsSuccess)
            {
                return new JsonResult(new { data = repoResponse.Content, status = 0 });
            }

            return new JsonResult(new { error_code = repoResponse.StatusCode, status = 1 });
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<JsonResult> GetProduct(int id)
        {
            var repoResponse = await _repo.GetProduct(id);

            Response.StatusCode = repoResponse.StatusCode;
            if (repoResponse.IsSuccess)
            {
                return new JsonResult(new { data = repoResponse.Content, status = 0 });
            }

            return new JsonResult(new { error_code = repoResponse.StatusCode, status = 1 });
        }

        [HttpGet("brief")]
        [AllowAnonymous]
        public async Task<JsonResult> GetBriefIdNamePairs()
        {
            var repoResponse = await _repo.GetBrief();

            Response.StatusCode = repoResponse.StatusCode;
            if (repoResponse.IsSuccess)
            {
                return new JsonResult(new { data = repoResponse.Content, status = 0 });
            }

            return new JsonResult(new { error_code = repoResponse.StatusCode, status = 1 });
        }

        [HttpPost("new")]
        public async Task<IActionResult> AddProduct([FromForm] ProductAddDto p)
        {
            var repoResponse = await _repo.AddProduct(p);

            if (repoResponse.IsSuccess)
            {
                Response.StatusCode = 200;
                return new JsonResult(new { data = "created", status = 0 });
            }

            Response.StatusCode = repoResponse.StatusCode;
            return new JsonResult(new { error_code = repoResponse.StatusCode, status = 1 });
        }

        [HttpPost("update/{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductAddDto p)
        {
            var repoResponse = await _repo.UpdateProduct(id, p);

            Response.StatusCode = repoResponse.StatusCode;
            if (repoResponse.IsSuccess)
            {
                return new JsonResult(new { data = "updated", status = 0 });
            }

            return new JsonResult(new { error_code = repoResponse.StatusCode, status = 1 });
        }

        [HttpPost("delete/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var repoResponse = await _repo.DeleteProduct(id);

            Response.StatusCode = repoResponse.StatusCode;
            if (repoResponse.IsSuccess)
            {
                return new JsonResult(new { data = "deleted", status = 0 });
            }

            return new JsonResult(new { error_code = repoResponse.StatusCode, status = 1 });
        }
    }
}
