using System;

namespace Acreator.Models
{
    public class OrderAddDto
    {
        public int ProductId { get; set; }
        public int Area { get; set; }
        public int FinalPrice { get; set; }
        public string Contact { get; set; }
        public string Details { get; set; }
    }
}