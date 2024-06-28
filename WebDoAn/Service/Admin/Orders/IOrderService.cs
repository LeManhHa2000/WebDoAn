using WebDoAn.Models;
using WebDoAn.Service.Admin.Orders.Dto;

namespace WebDoAn.Service.Admin.Orders
{
    public interface IOrderService
    {
        public Task<int> Create(Order order);
        public Task<int> Update(Order order);
        public List<Order> GetAll(GetInput input);
    }
}
