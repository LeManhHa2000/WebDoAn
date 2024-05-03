using System.ComponentModel.DataAnnotations;
using WebDoAn.Enums;

namespace WebDoAn.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }
        public string? Name { get; set; }
        public string? SerName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public UserEnum.RoleUser Role { get; set; }
        public string? Password { get; set; }
        public bool Active { get; set; }
    }
}
