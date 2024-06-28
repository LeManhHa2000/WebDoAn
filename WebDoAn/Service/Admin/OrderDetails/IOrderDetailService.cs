using WebDoAn.Models;

namespace WebDoAn.Service.Admin.OrderDetails
{
    public interface IOrderDetailService
    {
        public Task<bool> Create(OrderDetail orderDetail);
    }
}
