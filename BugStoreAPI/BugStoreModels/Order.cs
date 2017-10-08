
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugStoreModels
{
    [Table("Orders", Schema = "Store")]
    public class Order
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public DateTime? DateCreated { get; set; }

        public decimal LineItemTotal { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] TimeStamp { get; set; }

        public Product Product { get; set; }
    }
}
