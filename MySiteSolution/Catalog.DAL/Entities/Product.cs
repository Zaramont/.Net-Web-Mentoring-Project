using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Catalog.DAL.Entities
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Please enter product name.")]
        [StringLength(40, ErrorMessage = "Maximum lenght of product name is 40 symbols.")]
        public string ProductName { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }

        [StringLength(20)]
        public string QuantityPerUnit { get; set; }
        public decimal UnitPrice { get; set; }
        [Range(0, short.MaxValue, ErrorMessage = "Units in stock value can't be less than zero.")]
        public short UnitsInStock { get; set; }
        public short UnitsOnOrder { get; set; }
        public short ReorderLevel { get; set; }
        public bool Discontinued { get; set; }

    }
}