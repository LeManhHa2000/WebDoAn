using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebDoAn.Enums;

namespace WebDoAn.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime ShipDate { get; set; }
        public bool Pay { get; set; }
        public DateTime PaymentDate { get; set; }
        public OrderEnum.OrderStatus Status { get; set; }
        public string? Note { get; set; }

        //[ForeignKey("Shipper")]
        //public int ShipperId { get; set; }
        //public virtual Shipper? shipper { get; set; }

    }
}
