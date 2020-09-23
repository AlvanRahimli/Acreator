using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Acreator.Data;
using Acreator.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Acreator.Repositories
{
    public class ImagesRepo : IImagesRepo
    {
        private readonly AppDbContext _context;

        public ImagesRepo(AppDbContext context)
        {
            this._context = context;
        }

        public async Task<RepoResponse<bool>> AddImage(ImageAddDto newImage)
        {
            var path = UploadImage(newImage.ImageFile, newImage.Alt);

            var image = new Image()
            {
                Url = path,
                Alt = newImage.Alt,
                Purpose = newImage.Purpose
            };

            await _context.Images.AddAsync(image);
            var dbRes = await _context.SaveChangesAsync();

            if (dbRes > 0)
            {
                return new RepoResponse<bool>()
                {
                    Content = true,
                    IsSuccess = true,
                    StatusCode = 201
                };
            }

            return new RepoResponse<bool>()
            {
                Content = false,
                IsSuccess = false,
                StatusCode = 500
            };
        }

        public async Task<RepoResponse<bool>> DeleteImage(int id)
        {
            var image = await _context.Images.FirstOrDefaultAsync(i => i.Id == id);

            if (image != null)
            {
                _context.Remove(image);
            }
            else
            {
                return new RepoResponse<bool>()
                {
                    Content = false,
                    IsSuccess = false,
                    StatusCode = 404
                };
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

        public async Task<RepoResponse<List<Image>>> GetAll()
        {
            var images = await _context.Images.AsNoTracking().ToListAsync();

            if (images.Count > 0)
            {
                return new RepoResponse<List<Image>>()
                {
                    Content = images,
                    IsSuccess = true,
                    StatusCode = 200
                };
            }

            return new RepoResponse<List<Image>>()
            {
                Content = null,
                IsSuccess = false,
                StatusCode = 500
            };
        }

        

        public async Task<RepoResponse<List<Image>>> GetFiltered(ImagePurpose purpose)
        {
            var images = await _context.Images
                .AsNoTracking()
                .Where(i => i.Purpose == purpose)
                .ToListAsync();

            if (images.Count > 0)
            {
                return new RepoResponse<List<Image>>()
                {
                    Content = images,
                    IsSuccess = true,
                    StatusCode = 200
                };
            }

            return new RepoResponse<List<Image>>()
            {
                Content = null,
                IsSuccess = false,
                StatusCode = 500
            };
        }

        private string UploadImage(IFormFile image, string imageAlt)
        {
            string filePath;
            try
            {
                var file = image;
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine("/var/www/data", folderName);

                if (file.Length > 0)
                {
                    var oldFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fileName = string.Join(
                        '.', 
                        (imageAlt.ToLower() + (new Random()).Next(1000, 9999).ToString()).Replace(' ', '_'),
                        oldFileName.Split('.')[^1]
                    );
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
    }
}