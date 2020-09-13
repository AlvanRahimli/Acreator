using System.Collections.Generic;
using System.Threading.Tasks;
using Acreator.Models;

namespace Acreator.Repositories
{
    public interface IImagesRepo
    {
        Task<RepoResponse<List<Image>>> GetAll();
        Task<RepoResponse<List<Image>>> GetFiltered(ImagePurpose purpose);
        Task<RepoResponse<bool>> AddImage(ImageAddDto newImage);

    }
}