using System;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Acreator.Models;
using Acreator.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Acreator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsRepo _repo;

        public ProductsController(IProductsRepo repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<JsonResult> GetProducts()
        {
            var repoResponse = await _repo.GetProducts();
            if (repoResponse.IsSuccess)
            {
                Response.StatusCode = 200;
                return new JsonResult(new { data = repoResponse.Content, status = 0 });
            }

            Response.StatusCode = repoResponse.StatusCode;
            return new JsonResult(new { error_code = repoResponse.StatusCode, status = 1 });
        }

        [HttpPost("new")]
        public async Task<IActionResult> AddProduct([FromForm]ProductAddDto p)
        {
            string filePath;
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    filePath = dbPath;
                }
                else
                {
                    filePath = "no_image";
                }
            }
            catch (Exception)
            {
                filePath = "image_upload_error";
            }

            p.ImageUrl = filePath;
            var repoResponse = await _repo.AddProduct(p);

            if (repoResponse.IsSuccess)
            {
                Response.StatusCode = 200;
                return new JsonResult(new { data = "created", status = 0 });
            }

            Response.StatusCode = repoResponse.StatusCode;
            return new JsonResult(new { error_code = repoResponse.StatusCode, status = 1 });
        }
    }
}