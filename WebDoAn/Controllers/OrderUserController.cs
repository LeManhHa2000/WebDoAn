using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using WebDoAn.dbs;
using WebDoAn.ModelPrivew;
using WebDoAn.Models;
using WebDoAn.Service.Admin.OrderDetails;
using WebDoAn.Service.Admin.Orders;
using WebDoAn.Service.Admin.Orders.Dto;
using WebDoAn.Service.Admin.Products;
using WebDoAn.Service.Client.Carts;

namespace WebDoAn.Controllers
{
    public class OrderUserController : Controller
    {
        public readonly ICartService _cartService;
        private readonly IHttpContextAccessor _contxt;
        public readonly DoAnDbContext _db;
        public INotyfService _notyfService;
        public readonly IOrderService _orderService;
        public readonly IOrderDetailService _orderDetailService;
        public readonly IProductService _productService;

        public OrderUserController(ICartService cartService, IHttpContextAccessor contxt, DoAnDbContext db, INotyfService notyfService, IOrderService orderService,
            IOrderDetailService orderDetailService, IProductService productService)
        {
            _cartService = cartService;
            _contxt = contxt;
            _db = db;
            _notyfService = notyfService;
            _orderService = orderService;
            _orderDetailService = orderDetailService;
            _productService = productService;
        }

        public IActionResult Index(int id)
        {
            if (id != _contxt.HttpContext.Session.GetInt32("UserId"))
            {
                return NotFound();
            }
             ViewBag.UserId = id;
            return View();
        }

        public JsonResult GetAllOrder(GetInput input)
        {
            var list = _orderService.GetAll(input);
            return Json(list);
        }

        public IActionResult CreateOrderUser(int id)
        {
            if (id != _contxt.HttpContext.Session.GetInt32("UserId"))
            {
                return NotFound();
            }

            var listcart = _db.cart.Where(x => x.UserId == id).ToList();
            var listproductdb = _db.product.ToList();
            var listproduct = _productService.GetAllProDto(listproductdb);

            var listreturn = (from a in listcart
                              join b in listproduct on a.ProductId equals b.Id
                              select new CartViewModal
                              {
                                  Id = a.Id,
                                  Name = b.Name,
                                  Image = b.Image,
                                  Quantity = a.Quantity,
                                  Price = a.Price,
                                  Total = a.Quantity * a.Price,
                                  IsToMuch = a.Quantity > b.Quantity ? true : false,
                              }).OrderBy(x => x.Id).ToList();

            var sumTotal = listreturn.Sum(d => d.Total);
            ViewBag.ToTalSum = sumTotal;
            var user = _db.user.Where(x => x.Id == id).FirstOrDefault();

            ViewBag.ListCartModal = listreturn;
            ViewBag.UserInfor = user;

            return View();
        }

        public IActionResult Details(int id)
        {
            var order = _db.order.Where(x => x.Id == id).FirstOrDefault();
            if (order.UserId != _contxt.HttpContext.Session.GetInt32("UserId"))
            {
                return NotFound();
            }

            var listorderdetail = _db.orderDetail.Where(x => x.OrderId == id).ToList();
            var listproductbd = _db.product.ToList();
            var listproduct = _productService.GetAllProDto(listproductbd);

            var listreturn = (from a in listorderdetail
                              join b in listproduct on a.ProductId equals b.Id
                              select new OrderDetailViewModal
                              {
                                  Id = a.Id,
                                  Name = b.Name,
                                  Image = b.Image,
                                  Quantity = a.Quantity,
                                  Total = a.Total,
                                  Sum = a.Quantity * a.Total,
                                  ProductId = a.ProductId,
                                  OrderId = a.OrderId
                              }).OrderBy(x => x.Id).ToList();

            ViewBag.ToTalSum = order.Total;
            var user = _db.user.Where(x => x.Id == order.UserId).FirstOrDefault();

            ViewBag.ListOrderDetailModal = listreturn;
            ViewBag.UserInfor = user;

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder(Order order)
        {
            // Check lại số lượng hàng
            var listcart = _db.cart.Where(x => x.UserId == order.UserId).ToList();
            var listproduct = _db.product.ToList();

            var listcartset = (from a in listcart
                               join b in listproduct on a.ProductId equals b.Id
                               where a.Quantity > b.Quantity
                               select new Cart
                               {
                                   Id = a.Id,
                                   Quantity = b.Quantity,
                                   ProductId = a.ProductId,
                                   UserId = a.UserId,
                               }).ToList();
            
            // nếu số lượng sản phẩm đã thay đổi
            if(listcartset.Count > 0)
            {
                _notyfService.Error("Đã có sản phẩm thay đổi số lượng và không phù hợp với số lượng trong giỏ hàng của bạn và chúng tôi đã cập nhật nó cho phù hợp số lượng");

                foreach (var item in listcartset)
                {
                    await _cartService.UpdateMultil(item);
                }
                return RedirectToAction("Index", "Cart", new { id = _contxt.HttpContext.Session.GetInt32("UserId") });
            }

            // Nếu không thì tiếp tục tạo order

            var idOrder = await _orderService.Create(order);


            // Lấy ra chuỗi OrderDetail để tạo mới 
            var listOrderdetail = (from a in listcart
                              select new OrderDetail
                              {
                                  OrderId = idOrder,
                                  ProductId = a.ProductId,
                                  Quantity = a.Quantity,
                                  Total = a.Price,
                              }).ToList();
            // Tạo mới orderDetail
            foreach (var item in listOrderdetail)
            {
                await _orderDetailService.Create(item);
            }

            // Cập nhật lại số lượng sản phẩm
            var listproductnew = (from a in listcart
                              select new Product
                              {
                                  Id = a.ProductId,
                                  Quantity = a.Quantity,
                              }).ToList();

            // Cập nhật lại số lượng sản phẩm
            foreach(var pro in listproductnew)
            {
                await _productService.UpdateQuantityDH(pro);
            }

            // Xóa giỏ hàng
            foreach(var cart in listcart)
            {
                await _cartService.Delete(cart.Id);
            }

            _notyfService.Success("Tạo mới đơn hàng thành công!");
            return RedirectToAction("Index", "OrderUser", new { id = order.UserId });
        }
    }
}
