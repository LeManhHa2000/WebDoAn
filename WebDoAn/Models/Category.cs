using System.ComponentModel.DataAnnotations;

namespace WebDoAn.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
		public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
