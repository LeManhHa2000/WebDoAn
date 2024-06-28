using AspNetCoreHero.ToastNotification.Abstractions;
using System.Reflection.Metadata;
using WebDoAn.dbs;
using WebDoAn.Models;
using WebDoAn.Service.Admin.Orders.Dto;

namespace WebDoAn.Service.Admin.Orders
{
    public class OrderService : IOrderService
    {
        private readonly DoAnDbContext _db;
        public INotyfService _notyfService;

        public OrderService(DoAnDbContext db, INotyfService notyfService)
        {
            _db = db;
            _notyfService = notyfService;
        }

        public async Task<int> Create(Order order)
        {
            var date = DateTime.Now;
            var datecustom = DateTime.SpecifyKind(date, DateTimeKind.Utc);
            order.CreateTime = datecustom;
            order.Status = Enums.OrderEnum.OrderStatus.Waitting;
            order.PaymentMethod = Enums.OrderEnum.MethodPay.COD;

            _db.order.Add(order);
            await _db.SaveChangesAsync();
            return order.Id;
        }

        public List<Order> GetAll(GetInput input)
        {
            if(input.OrderId == 0)
            {
                var listorder = _db.order.Where(x => x.UserId == input.IdUser).OrderByDescending(x => x.Id).ToList();
                return listorder;
            }
            else
            {
                var listorder = _db.order.Where(x => x.UserId == input.IdUser && x.Id == input.OrderId).OrderByDescending(x => x.Id).ToList();
                return listorder;
            }
        }

        public async Task<int> Update(Order order)
        {
            throw new NotImplementedException();
        }
    }
}
