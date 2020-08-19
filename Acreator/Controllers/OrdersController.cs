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
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersRepo _repo;

        public OrdersController(IOrdersRepo repo)
        {
            this._repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var repoResponse = await _repo.GetOrders();

            Response.StatusCode = repoResponse.StatusCode;
            if (!repoResponse.IsSuccess)
            {
                return new JsonResult(new { error_code = repoResponse.StatusCode, status = 1 });
            }

            return new JsonResult(new { data = repoResponse.Content, status = 0 });
        }

        [HttpGet("filter/{status}")]
        public async Task<IActionResult> GetFiltered(OrderStatus status)
        {
            var repoResponse = await _repo.GetFiltered(status);

            Response.StatusCode = repoResponse.StatusCode;
            if (!repoResponse.IsSuccess)
            {
                return new JsonResult(new { error_code = repoResponse.StatusCode, status = 1 });
            }

            return new JsonResult(new { data = repoResponse.Content, status = 0 });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingle(int id)
        {
            var repoResponse = await _repo.GetOrder(id);

            Response.StatusCode = repoResponse.StatusCode;
            if (!repoResponse.IsSuccess)
            {
                return new JsonResult(new { error_code = repoResponse.StatusCode, status = 1 });
            }

            return new JsonResult(new { data = repoResponse.Content, status = 0 });
        }

        [HttpPost("new")]
        [AllowAnonymous]
        public async Task<IActionResult> AddOrder([FromForm] OrderAddDto newOrder)
        {
            var repoResponse = await _repo.AddOrder(newOrder);

            Response.StatusCode = repoResponse.StatusCode;
            if (!repoResponse.IsSuccess)
            {
                return new JsonResult(new { error_code = repoResponse.StatusCode, status = 1 });
            }

            return new JsonResult(new { data = repoResponse.Content, status = 0 });
        }

        [HttpPost("set-status/{id}")]
        public async Task<IActionResult> ChangeStatus(int id, OrderStatus status)
        {
            var repoResponse = await _repo.ChangeStatus(id, status);

            Response.StatusCode = repoResponse.StatusCode;
            if (!repoResponse.IsSuccess)
            {
                return new JsonResult(new { error_code = repoResponse.StatusCode, status = 1 });
            }

            return new JsonResult(new { data = repoResponse.Content, status = 0 });
        }
    }
}