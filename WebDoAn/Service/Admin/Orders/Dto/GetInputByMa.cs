using WebDoAn.Enums;

namespace WebDoAn.Service.Admin.Orders.Dto
{
    public class GetInputByMa
    {
        public string Code { get; set; }
        public OrderEnum.OrderStatus Status { get; set; }
    }
}
