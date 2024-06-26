using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebDoAn.Models
{
    public class Blog
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
		public string? Title { get; set; }
        public string? Description { get; set; }
        public string? SubDescription { get; set; }
        public string? ImgSrc { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User? user { get; set; }
    }
}
