using System.ComponentModel.DataAnnotations;

namespace WebDoAn.Enums
{
    public class OrderEnum
    {
        public enum OrderStatus : byte
        {
            [Display(Name = "Đã giao hàng")]
            Delivered = 0,
            [Display(Name = "Đơn hàng đang giao")]
            Delivering = 1,
            [Display(Name = "Đơn hàng bị hủy")]
            Delete = 2,

        }
    }
}
