using System.ComponentModel.DataAnnotations;
using WebDoAn.Enums;

namespace WebDoAn.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public UserEnum.RoleUser Role { get; set; } = UserEnum.RoleUser.Client;
        public UserEnum.GenderEnum Gender { get; set; }
		public string? Password { get; set; }
        public bool Active { get; set; } = true;
    }
}
