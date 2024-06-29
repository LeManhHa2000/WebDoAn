using AspNetCoreHero.ToastNotification.Abstractions;
using System.Reflection.Metadata;
using WebDoAn.dbs;
using WebDoAn.ModelPrivew;
using WebDoAn.Models;
using WebDoAn.Service.Admin.Orders.Dto;
using WebDoAn.Service.Admin.Products;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static WebDoAn.Enums.OrderEnum;

namespace WebDoAn.Service.Admin.Orders
{
    public class OrderService : IOrderService
    {
        private readonly DoAnDbContext _db;
        public INotyfService _notyfService;
        public readonly IProductService _productService;

        public OrderService(DoAnDbContext db, INotyfService notyfService, IProductService productService)
        {
            _db = db;
            _notyfService = notyfService;
            _productService = productService;
        }

        public async Task<int> Create(Order order)
        {
            var query = _db.order.ToList().Count();
            string ma;
            string sinhma(int i)
            {
                i++;
                if (i < 10) return "ĐH-" + "00" + Convert.ToString(i);
                else
                if (i >= 10 && i < 100) return "ĐH-" + "0" + Convert.ToString(i);
                else return "ĐH-" + Convert.ToString(i);
            }

            ma = sinhma(query);
            order.Code = ma;

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

        public List<OrderViewByAdmin> GetAllbyAdmin(GetInputByMa input)
        {
            if (input.Code == "all")
            {
                var listorder = _db.order.Where(x => x.Status == input.Status).OrderBy(x => x.Id).ToList();
                var listuser = _db.user.ToList();

                var listreturn = (from a in listorder
                                  join b in listuser on a.UserId equals b.Id
                                  select new OrderViewByAdmin
                                  {
                                      Id = a.Id,
                                      Code = a.Code,
                                      NameUser = b.FullName,
                                      CreateTime = a.CreateTime.Date.ToShortDateString(),
                                      UpdateTime = a.UpdateTime.Date.ToShortDateString(),
                                      ShipDate = a.ShipDate.Date.ToShortDateString(),
                                      Status = a.Status,
                                  }).OrderBy(x => x.Status).ToList();
                return listreturn;
            }
            else
            {
                var listorder = _db.order.Where(x => x.Status == input.Status && x.Code.ToLower().Contains(input.Code.ToLower())).OrderBy(x => x.Id).ToList();
                var listuser = _db.user.ToList();

                var listreturn = (from a in listorder
                                  join b in listuser on a.UserId equals b.Id
                                  select new OrderViewByAdmin
                                  {
                                      Id = a.Id,
                                      Code = a.Code,
                                      NameUser = b.FullName,
                                      CreateTime = a.CreateTime.Date.ToShortDateString(),
                                      UpdateTime = a.UpdateTime.Date.ToShortDateString(),
                                      ShipDate = a.ShipDate.Date.ToShortDateString(),
                                      Status = a.Status,
                                  }).OrderBy(x => x.Status).ToList();
                return listreturn;
            }
        }

        public async Task<Order> GetOrderbyId(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(Order order)
        {
            var orderold = _db.order.Where(x => x.Id == order.Id).FirstOrDefault();
            orderold.Status = order.Status;

            // Tao moi ngay cập nhật
            var date = DateTime.Now;
            var datecustom = DateTime.SpecifyKind(date, DateTimeKind.Utc);
            orderold.UpdateTime = datecustom;

            // Khi chỉnh trạng thái là đang giao hàng
            if (order.Status == OrderStatus.Delivering)
            {
                // Tao moi ngay giao hang
                var dategiaohang = DateTime.Now;
                var dategiaohangcustom = DateTime.SpecifyKind(date, DateTimeKind.Utc);
                orderold.ShipDate = dategiaohangcustom;
            }

            // Khi hủy
            else if(order.Status == OrderStatus.Reject)
            {
                // Lấy danh sách orderDetail
                var listorderdt = _db.orderDetail.Where(x => x.OrderId == order.Id).ToList();
                //Lấy danh sách sản phẩm
                var listproduct = _db.product.ToList();

                // lấy ra danh sách sản phẩm khi cập nhật lại số lượng
                var listspnew = (from a in listorderdt
                                 join b in listproduct on a.ProductId equals b.Id
                                 select new Product
                                 {
                                     Id = a.ProductId,
                                     Quantity = a.Quantity + b.Quantity,
                                 }).ToList();

                // Cập nhật lại số lượng sản phẩm khi hủy đơn hàng
                foreach(var item in listspnew )
                {
                    await _productService.UpdateQuantity(item);
                }
            }

            _db.order.Update(orderold);
            await _db.SaveChangesAsync();
            _notyfService.Success("Cập nhật thành công");
            return true;
        }
    }
}
