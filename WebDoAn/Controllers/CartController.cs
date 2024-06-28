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
                              }).OrderBy(x => x.Id).ToList();
            return View(listreturn);
        }

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

        [HttpPost]
        public async Task<ActionResult> DeleteToCart(int id)
        {
            var isCart = await _cartService.Delete(id);
            var code = new { Success = true, isXoa = isCart, code = 1 };
            return Json(code);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateToCart(int id, int soluong)
        {
            var cart = _db.cart.Where(x => x.Id == id).FirstOrDefault();
            cart.Quantity = soluong;
            var isCart = await _cartService.Update(cart);
            var code = new { Success = true, isUpdate = isCart, code = 1 };
            return Json(code);
        }
    }
}
