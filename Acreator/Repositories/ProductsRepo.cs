using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Acreator.Data;
using Acreator.Models;
using Microsoft.AspNetCore.Http;
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

            if (products == null || products.Count == 0)
            {
                return new RepoResponse<List<Product>>()
                {
                    Content = null,
                    IsSuccess = false,
                    StatusCode = 404
                };
            }

            return new RepoResponse<List<Product>>()
            {
                Content = products,
                IsSuccess = true,
                StatusCode = 200
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
                ImageUrl = UploadImage(newProduct.Image, newProduct.Name),
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
                .FirstOrDefaultAsync(p => p.Id == id);

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
            if (updatedProduct.Image != null)
            {
                string imageUrl = UploadImage(updatedProduct.Image, updatedProduct.Name);
                existingProduct.ImageUrl = imageUrl;
            }

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

        private string UploadImage(IFormFile image, string product_name)
        {
            string filePath;
            try
            {
                var file = image;
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var oldFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fileName = string.Join('.', product_name.ToLower().Replace(' ', '_'), oldFileName.Split('.')[^1]);
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    filePath = dbPath;
                }
                else
                {
                    filePath = "no_image";
                }
            }
            catch (Exception)
            {
                filePath = "image_upload_error";
            }

            return filePath;
        }

        public async Task<RepoResponse<List<Product>>> GetFiltered(ProductType type)
        {
            var products = await _context.Products
                .Include(p => p.Measurement)
                .Where(p => p.Type == type)
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
    }
}