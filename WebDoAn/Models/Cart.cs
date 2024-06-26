using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebDoAn.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public int Quantity { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product? product { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User? user { get; set; }
    }
}
