using System.ComponentModel.DataAnnotations;

namespace WebDoAn.Enums
{
    public class OrderEnum
    {
        public enum OrderStatus : byte
        {
			[Display(Name = "Chờ xác nhận")]
			Waitting = 0,
			[Display(Name = "Được xác nhận")]
			Confirm = 1,
			[Display(Name = "Đang giao hàng")]
            Delivering = 2,
            [Display(Name = "Giao hàng thành công")]
			Delivered = 3,
			[Display(Name = "Hoàn thành")]
			Complete = 4,
			[Display(Name = "Đơn hàng bị hủy")]
            Reject = 5,

        }

		public enum MethodPay : byte
		{
			[Display(Name = "Thanh toán tiền mặt khi nhận hàng (COD)")]
			COD = 0,
		}
    }
}
