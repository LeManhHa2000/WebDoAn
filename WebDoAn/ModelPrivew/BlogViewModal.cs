using System.ComponentModel.DataAnnotations.Schema;
using WebDoAn.Models;

namespace WebDoAn.ModelPrivew
{
    public class BlogViewModal
    {
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? SubDescription { get; set; }
        public IFormFile? Photo { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User? user { get; set; }
    }
}
