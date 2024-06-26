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
        public enum GenderEnum : byte
        {
			[Display(Name = "Nam")]
			Boy = 0,
			[Display(Name = "Nữ")]
			Girl = 1,
			[Display(Name = "Khác")]
			Other = 0,

		}

	}
}
