using System.Collections.Generic;
using System.Threading.Tasks;
using Acreator.Models;

namespace Acreator.Repositories
{
    public interface IProductsRepo
    {
        Task<RepoResponse<List<Product>>> GetProducts();
        Task<RepoResponse<Product>> GetProduct(int id);
        Task<RepoResponse<bool>> AddProduct(ProductAddDto newProduct);
        Task<RepoResponse<bool>> UpdateProduct(int id, ProductAddDto updatedProduct);
        Task<RepoResponse<bool>> DeleteProduct(int id);
    }
}