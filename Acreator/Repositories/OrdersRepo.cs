using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acreator.Data;
using Acreator.Models;
using Microsoft.EntityFrameworkCore;

namespace Acreator.Repositories
{
    public class OrdersRepo : IOrdersRepo
    {
        private readonly AppDbContext _context;

        public OrdersRepo(AppDbContext context)
        {
            this._context = context;
        }
        public async Task<RepoResponse<bool>> AddOrder(OrderAddDto newOrder)
        {
            var order = new Order()
            {
                Date = DateTime.Now,
                Contact = newOrder.Contact,
                Details = newOrder.Details,
                ProductId = newOrder.ProductId,
                Status = OrderStatus.Pending,
                Area = newOrder.Area,
                FinalPrice = newOrder.FinalPrice
            };

            await _context.Orders.AddAsync(order);
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

        public async Task<RepoResponse<bool>> ChangeStatus(int id, OrderStatus status)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(p => p.Id == id);
            
            if (order == null)
            {
                return new RepoResponse<bool>()
                {
                    Content = false,
                    IsSuccess = false,
                    StatusCode = 404
                };
            }

            order.Status = status;
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

        public async Task<RepoResponse<List<Order>>> GetFiltered(OrderStatus status)
        {
            var orders = await _context.Orders
                .Include(o => o.Product)
                .Where(p => p.Status == status)
                .ToListAsync();
            
            if (orders == null || orders.Count == 0)
            {
                return new RepoResponse<List<Order>>()
                {
                    Content = null,
                    IsSuccess = false,
                    StatusCode = 404
                };
            }

            return new RepoResponse<List<Order>>()
            {
                Content = orders,
                IsSuccess = true,
                StatusCode = 200
            };
        }

        public async Task<RepoResponse<Order>> GetOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Product)
                .FirstOrDefaultAsync(p => p.Id == id);
            
            if (order == null)
            {
                return new RepoResponse<Order>()
                {
                    Content = null,
                    IsSuccess = false,
                    StatusCode = 404
                };
            }

            return new RepoResponse<Order>()
            {
                Content = order,
                IsSuccess = true,
                StatusCode = 200
            };
        }

        public async Task<RepoResponse<List<Order>>> GetOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.Product)
                .ToListAsync();
            
            if (orders == null)
            {
                return new RepoResponse<List<Order>>()
                {
                    Content = null,
                    IsSuccess = false,
                    StatusCode = 404
                };
            }

            return new RepoResponse<List<Order>>()
            {
                Content = orders,
                IsSuccess = true,
                StatusCode = 200
            };
        }
    }
}