using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebDoAn.Models
{
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
        public string? ImgProduct { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public virtual Order? order { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product? product { get; set; }

    }
}
