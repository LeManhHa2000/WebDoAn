using WebDoAn.ModelPrivew;
using WebDoAn.Models;
using WebDoAn.Service.Admin.Orders.Dto;

namespace WebDoAn.Service.Admin.Orders
{
    public interface IOrderService
    {
        public Task<int> Create(Order order);
        public Task<bool> Update(Order order);
        public List<Order> GetAll(GetInput input);
        public List<OrderViewByAdmin> GetAllbyAdmin(GetInputByMa input);
        public Task<Order> GetOrderbyId(int id);

    }
}
