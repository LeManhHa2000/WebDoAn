using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using WebDoAn.dbs;
using WebDoAn.ModelPrivew;
using WebDoAn.Models;
using WebDoAn.Service.Client.Carts;

namespace WebDoAn.Controllers
{
    public class CartController : Controller
    {
        public readonly ICartService _cartService;
        private readonly IHttpContextAccessor _contxt;
        public readonly DoAnDbContext _db;
        public INotyfService _notyfService;

        public CartController(ICartService cartService, IHttpContextAccessor contxt, DoAnDbContext db, INotyfService notyfService)
        {
            _cartService = cartService;
            _contxt = contxt;
            _db = db;
            _notyfService = notyfService;
        }

        public IActionResult Index(int id)
        {
            if(id != _contxt.HttpContext.Session.GetInt32("UserId"))
            {
                return NotFound();
            }

            var listcart = _db.cart.Where(x => x.UserId == id).ToList();
            var listproduct = _db.product.ToList();

            var listreturn = (from a in listcart
                              join b in listproduct on a.ProductId equals b.Id
                              select new CartViewModal
                              {
                                  Id = a.Id,
                                  Name = b.Name,
                                  Image = b.Image,
                                  Quantity = a.Quantity,
                                  Price = b.Price,
                                  Total = a.Quantity * b.Price,
                                  ProductId = a.ProductId,
                                  IsToMuch = a.Quantity > b.Quantity ? true : false,
                              }).OrderBy(x => x.Id).ToList();
            return View(listreturn);
        }

        // Cập nhật cart
        [HttpPost]
        public async Task<ActionResult> AddToCart(int idpro, int quantt, int userid)
        {
            Cart cart = new Cart
            {
                ProductId = idpro,
                Quantity = quantt,
                UserId = userid
            };
            var isCart = await _cartService.AddToCart(cart);
            var code = new {Success = true, ishang = isCart, code = 1};
            return Json(code);
        }

        // Khi nhấn nút tăng + 1 sản phẩm
        public async Task<ActionResult> CreToCart(int id, int soluong, int proid)
        {
            Cart cart = new Cart
            {
                Id = id,
                Quantity = soluong,
                ProductId= proid,
            };
            var isCart = await _cartService.CreToCart(cart);
            var code = new { Success = true, iscre = isCart, code = 1 };
            return Json(code);
        }

        // Khi nhấn nút giảm
        public async Task<ActionResult> DesToCart(int id, int soluong ,int proid)
        {
            Cart cart = new Cart
            {
                Id = id,
                Quantity = soluong,
                ProductId = proid,
            };
            var isCart = await _cartService.DesToCart(cart);
            var code = new { Success = true, isdes = isCart, code = 1 };
            return Json(code);
        }

        // Khi nhập input
        [HttpPost]
        public async Task<ActionResult> UpdateToCart(int id, int soluong)
        {
            var code = new { Success = false, isUpdate = false, soluong = 0 };
            var cart = _db.cart.Where(x => x.Id == id).FirstOrDefault();
            var soluongcu = cart.Quantity;

            cart.Quantity = soluong;
            var isCart = await _cartService.Update(cart);
            if(isCart == true)
            {
                code = new { Success = true, isUpdate = true, soluong = 1 };
            }
            else
            {
                code = new { Success = true, isUpdate = false, soluong = soluongcu };
            }
            return Json(code);
        }


        [HttpPost]
        public async Task<ActionResult> DeleteToCart(int id)
        {
            var isCart = await _cartService.Delete(id);
            var code = new { Success = true, isXoa = isCart, code = 1 };
            return Json(code);
        }

        public async Task<ActionResult> CheckBeforCreateOrder()
        {
            var code = new { Success = false, isNot = false, soluong = 0 };
            var listcart = _db.cart.Where(x => x.UserId == _contxt.HttpContext.Session.GetInt32("UserId")).ToList();
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
            if(listcartset.Count > 0)
            {
                _notyfService.Warning("Đã có sản phẩm thay đổi số lượng và không phù hợp với số lượng trong giỏ hàng của bạn và chúng tôi đã cập nhật nó cho phù hợp số lượng");

                foreach(var item in listcartset) { 
                    await _cartService.UpdateMultil(item);
                }
                return RedirectToAction("Index", "Cart", new { id = _contxt.HttpContext.Session.GetInt32("UserId") });
            }

            return RedirectToAction("CreateOrderUser", "OrderUser", new { id = _contxt.HttpContext.Session.GetInt32("UserId") });
        }
    }
}
