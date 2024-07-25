using Microsoft.AspNetCore.Mvc;
using WebDoAn.Authentications;
using WebDoAn.dbs;
using WebDoAn.ModelPrivew;
using WebDoAn.Models;
using WebDoAn.Service.Admin.Products;

namespace WebDoAn.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public readonly DoAnDbContext _db;
        private readonly IHttpContextAccessor _contxt;
        public readonly IProductService _productService;

        public HomeController(DoAnDbContext db, IHttpContextAccessor contxt, IProductService productService) {  
            _db = db;
            _contxt = contxt;
            _productService = productService;
        }
        [Authentication]
        public IActionResult Index()
        {
            // Lấy ra người dùng có tổng lớn nhất
            var listuser = _db.user.ToList();
            var listorder = _db.order.Where(x => x.Status == Enums.OrderEnum.OrderStatus.Complete).ToList();

            var listuserorder = (from i in listorder
                                 group i by i.UserId into grp
                                 select new { UserId = grp.Key, ToTal = grp.Sum(i => i.Total) }).ToList().OrderByDescending(x => x.ToTal).Take(5);

            var listUserReturn = (from a in listuserorder
                                  join b in listuser on a.UserId equals b.Id
                                  select new UserRankViewModal
                                  {
                                      Name = b.FullName,
                                      ToTal = a.ToTal,
                                      PhoneNumber = b.PhoneNumber
                                  }).ToList();

            // lấy ra sản phẩm phẩm có lượt mua nhiều nhất
            var listorderDetail = _db.orderDetail.ToList();
            var listproFirstbd = _db.product.ToList();
            var listproFirst = _productService.GetAllProDto(listproFirstbd);

            var listorderDtSc = (from a in listorderDetail
                                 join b in listorder on a.OrderId equals b.Id
                                 select new OrderDetail
                                 {
                                     Id = a.Id,
                                     ProductId = a.ProductId,
                                     Quantity = a.Quantity,
                                 }).ToList();

            var listoderGop = (from i in listorderDtSc
                               group i by i.ProductId into grp
                               select new { ProductId = grp.Key, ToTal = grp.Sum(i => i.Quantity) }).ToList().OrderByDescending(x => x.ToTal).Take(5);

            var listproReturn = (from c in listoderGop
                                 join d in listproFirst on c.ProductId equals d.Id
                                 select new ProductRankViewModal
                                 {
                                     Name = d.Name,
                                     ImgSrc = d.Image,
                                     SoLuong = c.ToTal
                                 }).ToList();

            // Lấy số lượng đơn hàng
            var countOrderAll = _db.order.ToList().Count();
            var countOrderWait = _db.order.Where(x => x.Status == Enums.OrderEnum.OrderStatus.Waitting).ToList().Count();
            var countOrderComplete = _db.order.Where(x => x.Status == Enums.OrderEnum.OrderStatus.Complete).ToList().Count();
            var countOrderReject = _db.order.Where(x => x.Status == Enums.OrderEnum.OrderStatus.Reject).ToList().Count();

            ViewBag.countOrderAll = countOrderAll;
            ViewBag.countOrderWait = countOrderWait;
            ViewBag.countOrderComplete = countOrderComplete;
            ViewBag.countOrderReject = countOrderReject;

            ViewBag.ListUserRank = listUserReturn;
            ViewBag.ListProductRank = listproReturn;
            return View();
        }

        [HttpPost]
        public List<object> GetSumTotalOrder()
        {
            List<object> result = new List<object>();
            var listorder = _db.order.Where(x => x.Status == Enums.OrderEnum.OrderStatus.Complete).ToList();

            // Tạo mới 1 mảng array int theo 12 tháng
            var countarray = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            // Lấy ra tổng giá hóa đơn theo tháng 
            var q = (from i in listorder
                    group i by i.CreateTime.Date.Month into grp
                    select new { Month = grp.Key, Count = grp.Sum(i => i.Total) }).ToList();


            // Thêm vào mảng array với thứ tự bằng tháng tương ứng
            for (int i = 1; i< countarray.Count; i++)
            {
                for(var j = 0; j < q.Count; j++)
                {
                    if(i == q[j].Month)
                    {
                        countarray[i-1] = q[j].Count;
                    }
                }
            }

            result.Add(countarray);

            return result;

        }
    }
}
