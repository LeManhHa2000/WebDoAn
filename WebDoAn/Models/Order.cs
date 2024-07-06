using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebDoAn.Enums;

namespace WebDoAn.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
		public string? Code { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
		public DateTime ShipDate { get; set; }
        public int Total { get; set; }
        public OrderEnum.OrderStatus Status { get; set; }
		public OrderEnum.MethodPay PaymentMethod { get; set; }
		public string? Note { get; set; }
		public string? AddressReceive { get; set; }

        [ForeignKey("User")]
		public int UserId { get; set; }
		public virtual User? user { get; set; }

		//[ForeignKey("Shipper")]
		//public int ShipperId { get; set; }
		//public virtual Shipper? shipper { get; set; }

	}
}
