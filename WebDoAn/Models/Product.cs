using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebDoAn.Enums;

namespace WebDoAn.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ShortDescription { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? Tags { get; set; }
        public ProductEnum.TypeProduct TypeProduct { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual Category? category { get; set; }

    }
}
