using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugStoreModels
{
    [Table("Products", Schema = "Store")]
    public class Product
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        [Column(TypeName = "money")]
        public decimal CurrentPrice { get; set; }

        [StringLength(3800)]
        public string Description { get; set; }

        public bool IsFeatured { get; set; }

        [StringLength(50)]
        public string ModelName { get; set; }

        [StringLength(50)]
        public string ModelNumber { get; set; }

        [StringLength(150)]
        public string ProductImage { get; set; }

        [StringLength(150)]
        public string ProductImageLarge { get; set; }

        [StringLength(150)]
        public string ProductImageThumb { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] TimeStamp { get; set; }

        [Column(TypeName = "money")]
        public decimal UnitCost { get; set; }

        public int UnitsInStock { get; set; }
    }
}
