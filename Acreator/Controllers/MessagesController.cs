using System;
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
    public class MessagesController : ControllerBase
    {
        private readonly IMessagesRepo _repo;

        public MessagesController(IMessagesRepo repo)
        {
            this._repo = repo;
        }

        [AllowAnonymous]
        [HttpGet("filter/{status}")]
        public async Task<IActionResult> GetFiltered(MessageStatus status)
        {
            var repoResponse = await _repo.GetFiltered(status);

            Response.StatusCode = repoResponse.StatusCode;
            if (!repoResponse.IsSuccess)
            {
                return new JsonResult(new { error_code = repoResponse.StatusCode, status = 1 });
            }

            return new JsonResult(new { data = repoResponse.Content, status = 0 });
        }

        [AllowAnonymous]
        [HttpGet]
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

        [AllowAnonymous]
        [HttpPost("new")]
        public async Task<IActionResult> AddMessage(Message newMsg)
        {
            newMsg.Date = DateTime.Now;
            newMsg.Status = MessageStatus.Pending;
            var repoResponse = await _repo.AddMessage(newMsg);

            Response.StatusCode = repoResponse.StatusCode;
            if (!repoResponse.IsSuccess)
            {
                return new JsonResult(new { error_code = repoResponse.StatusCode, status = 1 });
            }

            return new JsonResult(new { data = repoResponse.Content, status = 0 });
        }

        [HttpPost("set-status/{id}")]
        public async Task<IActionResult> SetStatus(int id, MessageStatus status)
        {
            var repoResponse = await _repo.SetStatus(id, status);

            Response.StatusCode = repoResponse.StatusCode;
            if (!repoResponse.IsSuccess)
            {
                return new JsonResult(new { error_code = repoResponse.StatusCode, status = 1 });
            }

            return new JsonResult(new { data = repoResponse.Content, status = 0 });
        }
    }
}