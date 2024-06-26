using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebDoAn.Models
{
	public class Address
	{
		[Key]
		public int Id { get; set; }
		public string? AddressName { get; set; }
		public string? FullName { get; set; }
		public string? PhoneNumber { get; set; }

		[ForeignKey("User")]
		public int UserId { get; set; }
		public virtual User? user { get; set; }
	}
}
