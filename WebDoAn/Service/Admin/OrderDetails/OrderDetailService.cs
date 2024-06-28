using WebDoAn.dbs;
using WebDoAn.Models;

namespace WebDoAn.Service.Admin.OrderDetails
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly DoAnDbContext _db;
        public OrderDetailService(DoAnDbContext db) {
            _db = db; 
        }
        public async Task<bool> Create(OrderDetail orderDetail)
        {
            var date = DateTime.Now;
            var datecustom = DateTime.SpecifyKind(date, DateTimeKind.Utc);
            orderDetail.CreateTime = datecustom;

            _db.orderDetail.Add(orderDetail);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
