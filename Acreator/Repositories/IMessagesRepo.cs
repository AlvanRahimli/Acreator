using System.Collections.Generic;
using System.Threading.Tasks;
using Acreator.Models;

namespace Acreator.Repositories
{
    public interface IMessagesRepo
    {
        Task<RepoResponse<List<Message>>> GetAll();
        Task<RepoResponse<List<Message>>> GetFiltered(MessageStatus status);
        Task<RepoResponse<bool>> AddMessage(Message message);
        Task<RepoResponse<bool>> SetStatus(int id, MessageStatus status);
    }
}