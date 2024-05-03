using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebDoAn.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }
        public string? Text { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User? user { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product? product { get; set; }
    }
}
