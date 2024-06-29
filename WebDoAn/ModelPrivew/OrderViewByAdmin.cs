using WebDoAn.Enums;

namespace WebDoAn.ModelPrivew
{
    public class OrderViewByAdmin
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? NameUser { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string CreateTime { get; set; }
        public string UpdateTime { get; set; }
        public string ShipDate { get; set; }
        public int Total { get; set; }
        public OrderEnum.OrderStatus Status { get; set; }
        public OrderEnum.MethodPay PaymentMethod { get; set; }
        public string? Note { get; set; }
    }
}
