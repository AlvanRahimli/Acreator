using System.Collections.Generic;
using System.Threading.Tasks;
using Acreator.Models;

namespace Acreator.Repositories
{
    public interface IOrdersRepo
    {
        Task<RepoResponse<List<Order>>> GetOrders();
        Task<RepoResponse<Order>> GetOrder(int id);
        Task<RepoResponse<bool>> AddOrder(OrderAddDto newOrder);
        Task<RepoResponse<bool>> ChangeStatus(int id, OrderStatus status);
        Task<RepoResponse<List<Order>>> GetFiltered(OrderStatus status);
    }
}