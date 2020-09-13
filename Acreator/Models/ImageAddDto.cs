using Microsoft.AspNetCore.Http;

namespace Acreator.Models
{
    public class ImageAddDto
    {
        public string Alt { get; set; }
        public IFormFile ImageFile { get; set; }
        public ImagePurpose Purpose { get; set; }
    }
}