using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acreator.Data;
using Acreator.Models;
using Microsoft.EntityFrameworkCore;

namespace Acreator.Repositories
{
    public class MessagesRepo : IMessagesRepo
    {
        private readonly AppDbContext _context;
        public MessagesRepo(AppDbContext context)
        {
            this._context = context;
        }

        public async Task<RepoResponse<bool>> AddMessage(Message message)
        {
            await _context.Messages.AddAsync(message);
            var res = await _context.SaveChangesAsync();

            if (res > 0) 
            {
                return new RepoResponse<bool>()
                {
                    Content = true,
                    IsSuccess = true,
                    StatusCode = 200
                };
            }

            return new RepoResponse<bool>()
            {
                Content = false,
                IsSuccess = false,
                StatusCode = 500
            };
        }

        public async Task<RepoResponse<List<Message>>> GetAll()
        {
            var msgs = await _context.Messages
                .AsNoTracking()
                .ToListAsync();

            if (msgs.Count > 0 || msgs != null) 
            {
                return new RepoResponse<List<Message>>()
                {
                    Content = msgs,
                    IsSuccess = true,
                    StatusCode = 200
                };
            }

            return new RepoResponse<List<Message>>()
            {
                Content = null,
                IsSuccess = false,
                StatusCode = 500
            };
        }

        public async Task<RepoResponse<List<Message>>> GetFiltered(MessageStatus status)
        {
            var msgs = await _context.Messages
                .AsNoTracking()
                .Where(m => m.Status == status)
                .ToListAsync();

            if (msgs.Count > 0 || msgs != null) 
            {
                return new RepoResponse<List<Message>>()
                {
                    Content = msgs,
                    IsSuccess = true,
                    StatusCode = 200
                };
            }

            return new RepoResponse<List<Message>>()
            {
                Content = null,
                IsSuccess = false,
                StatusCode = 500
            };
        }

        public async Task<RepoResponse<bool>> SetStatus(int id, MessageStatus status)
        {
            var msg = await _context.Messages
                .FirstOrDefaultAsync(msg => msg.Id == id);
            
            if (msg == null)
            {
                return new RepoResponse<bool>()
                {
                    Content = false,
                    IsSuccess = false,
                    StatusCode = 404
                };
            }

            msg.Status = status;
            var res = await _context.SaveChangesAsync();

            if (res > 0) 
            {
                return new RepoResponse<bool>()
                {
                    Content = true,
                    IsSuccess = true,
                    StatusCode = 200
                };
            }

            return new RepoResponse<bool>()
            {
                Content = false,
                IsSuccess = false,
                StatusCode = 500
            };
        }
    }
}