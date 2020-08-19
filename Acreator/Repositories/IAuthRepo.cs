using System.Threading.Tasks;
using Acreator.Models;

namespace Acreator.Repositories
{
    public interface IAuthRepo
    {
        Task<RepoResponse<AdminReturnDto>> Login(Admin admin);
    }
}