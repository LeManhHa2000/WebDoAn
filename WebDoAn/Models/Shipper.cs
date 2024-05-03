using System.ComponentModel.DataAnnotations;

namespace WebDoAn.Models
{
    public class Shipper
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
