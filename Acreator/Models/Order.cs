using System;

namespace Acreator.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int ProductId { get; set; }
        public int Area { get; set; }
        public int FinalPrice { get; set; }
        public string Contact { get; set; }
        public string Details { get; set; }
        public OrderStatus Status { get; set; }
        public Product Product { get; set; }
    }
}