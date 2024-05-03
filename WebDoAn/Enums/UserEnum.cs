using System.ComponentModel.DataAnnotations;

namespace WebDoAn.Enums
{
    public class UserEnum
    {
        public enum RoleUser : byte
        {
            [Display(Name = "Admin")]
            Admin = 0,

            [Display(Name = "Khách hàng")]
            Client = 1,

        }
    }
}
