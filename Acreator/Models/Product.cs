namespace Acreator.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public int Price { get; set; }
        public ProductType Type { get; set; }
        public string Color { get; set; }
        public string Desc { get; set; }
        public int MeasurementId { get; set; }
        public Measurement Measurement { get; set; }
    }
}