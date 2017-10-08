using System;

namespace BugStoreModels.ViewModels
{
    public class OrderProductInfo
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime? DateCreated { get; set; }
        public decimal LineItemTotal { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public byte[] TimeStamp { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal CurrentPrice { get; set; }
        public string Description { get; set; }
        public bool IsFeatured { get; set; }
        public string ModelName { get; set; }
        public string ModelNumber { get; set; }
        public string ProductImage { get; set; }
        public decimal UnitCost { get; set; }
        public int UnitsInStock { get; set; }
    }
}
