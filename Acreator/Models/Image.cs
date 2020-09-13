namespace Acreator.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Alt { get; set; }
        public ImagePurpose Purpose { get; set; }
    }
}