using System.Collections.Generic;
using System.Threading.Tasks;
using Acreator.Data;
using Acreator.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Acreator.Repositories
{
    public class ProductsRepo : IProductsRepo
    {
        private readonly AppDbContext _context;

        public ProductsRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<RepoResponse<List<Product>>> GetProducts()
        {
            var products = await _context.Products
                .Include(p => p.Measurement)
                .ToListAsync();
            if (products != null)
            {
                return new RepoResponse<List<Product>>()
                {
                    Content = products,
                    IsSuccess = true
                };
            }
            return new RepoResponse<List<Product>>()
            {
                Content = null,
                IsSuccess = false,
                StatusCode = 404
            };
        }

        public async Task<RepoResponse<Product>> GetProduct(int id)
        {
            var product = await _context.Products
                .Include(p => p.Measurement)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
            return new RepoResponse<Product>()
            {
                Content = product,
                IsSuccess = product != null,
                StatusCode = product != null ? 200 : 404
            };
        }

        public async Task<RepoResponse<bool>> AddProduct([FromForm]ProductAddDto newProduct)
        {
            var newP = new Product()
            {
                ImageUrl = newProduct.ImageUrl,
                Name = newProduct.Name,
                Price = newProduct.Price,
                Type = newProduct.Type,
                Measurement = new Measurement()
                {
                    Width = newProduct.Width,
                    Height = newProduct.Height
                }
            };
            await _context.AddAsync(newP);
            var dbRes = await _context.SaveChangesAsync();
            if (dbRes > 0)
            {
                return new RepoResponse<bool>()
                {
                    Content = true,
                    IsSuccess = true
                };
            }

            return new RepoResponse<bool>()
            {
                Content = false,
                IsSuccess = false,
                StatusCode = 500
            };
        }

        public async Task<RepoResponse<bool>> UpdateProduct(int id, ProductAddDto updatedProduct)
        {
            var existingProduct = await _context.Products
                .Include(p => p.Measurement)
                .FirstOrDefaultAsync(predicate => predicate.Id == id);

            if (existingProduct == null)
            {
                return new RepoResponse<bool>()
                {
                    Content = false,
                    IsSuccess = false,
                    StatusCode = 404
                };
            }

            existingProduct.Name = updatedProduct.Name;
            existingProduct.Price = updatedProduct.Price;
            existingProduct.Type = updatedProduct.Type;
            existingProduct.Measurement.Width = updatedProduct.Width;
            existingProduct.Measurement.Height = updatedProduct.Height;

        }

        public async Task<RepoResponse<bool>> DeleteProduct(int id)
        {
            var p = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id);
            if (p == null)
            {
                return new RepoResponse<bool>()
                {
                    Content = false,
                    IsSuccess = false,
                    StatusCode = 404
                };
            }

            _context.Products.Remove(p);
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