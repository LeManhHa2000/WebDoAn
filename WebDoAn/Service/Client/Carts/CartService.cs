using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.EntityFrameworkCore;
using WebDoAn.dbs;
using WebDoAn.Models;

namespace WebDoAn.Service.Client.Carts
{
    public class CartService : ICartService
    {
        private readonly DoAnDbContext _db;
        public INotyfService _notyfService;

        public CartService(DoAnDbContext db, INotyfService notyfService)
        {
            _db = db;
            _notyfService = notyfService;
        }

        public async Task<bool> AddToCart(Cart cart)
        {
            var iscart = _db.cart.Where(x => x.UserId == cart.UserId && x.ProductId == cart.ProductId).ToList();
            if(iscart.Count == 0)
            {
                var quantitySoluong = _db.product.Where(x => x.Id == cart.ProductId).Select(x => x.Quantity).FirstOrDefault();
                if(cart.Quantity > quantitySoluong)
                {
                    _notyfService.Warning("Số lượng sản phẩm không đủ, vui lòng nhập lại số lượng");
                    return false;
                }
                else
                {
                    _db.cart.Add(cart);
                    await _db.SaveChangesAsync();
                    _notyfService.Success("Thêm vào giỏ hàng thành công");
                    return true;
                }
            }
            else
            {
                var soluongmoi = cart.Quantity + iscart[0].Quantity;
                var quantitySoluong = _db.product.Where(x => x.Id == cart.ProductId).Select(x => x.Quantity).FirstOrDefault();
                if(soluongmoi > quantitySoluong)
                {
                    _notyfService.Warning("Số lượng sản phẩm không đủ, vui lòng nhập lại số lượng");
                    return false;
                }
                else
                {
                    var cartitem = _db.cart.Where(x => x.UserId == cart.UserId && x.ProductId == cart.ProductId).FirstOrDefault();
                    cartitem.Quantity = cartitem.Quantity + cart.Quantity;
                    _db.cart.Update(cartitem);
                    await _db.SaveChangesAsync();
                    _notyfService.Success("Thêm vào giỏ hàng thành công");
                    return true;
                }
            }
        }

        public async Task<bool> Delete(int id)
        {
            var cart = _db.cart.Find(id);
            if (cart == null)
            {
                return false;
            }
            else
            {
                _db.cart.Remove(cart);
                await _db.SaveChangesAsync();
                return true;
            }
        }

        // Tăng thêm 1 sản phẩm
        public async Task<bool> CreToCart(Cart cart)
        {
            // số lượng sản phẩm
            var proquantity = _db.product.Where(x => x.Id == cart.ProductId).Select(x => x.Quantity).FirstOrDefault();
            // số lượng sản phẩm trong giỏ hàng + 1
            var quantitycart = cart.Quantity + 1;

            if(quantitycart > proquantity)
            {
                return false;
            }
            else
            {
                var cartnew = _db.cart.Where(x => x.Id == cart.Id).FirstOrDefault();
                cartnew.Quantity = cartnew.Quantity + 1;
                _db.cart.Update(cartnew);
                await _db.SaveChangesAsync();
                return true;
            }
        }

        //Giảm đi 1 sản phẩm
        public async Task<bool> DesToCart(Cart cart)
        {
            // Lấy số lượng nhập - 1
            var quantitycart = cart.Quantity - 1;
            if(quantitycart > 0)
            {
                var cartnew = _db.cart.Where(x => x.Id == cart.Id).FirstOrDefault();
                cartnew.Quantity = cartnew.Quantity - 1;
                _db.cart.Update(cartnew);
                await _db.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<Cart>> GetCart(int id)
        {
            var cart = await _db.cart.Where(x => x.UserId == id).ToListAsync();
            return cart;
        }

        public async Task<bool> Update(Cart cart)
        {
            var iscart = _db.cart.Where(x => x.Id == cart.Id).FirstOrDefault();

            var soluongsanpham = _db.product.Where(x => x.Id == iscart.ProductId).Select(x => x.Quantity).FirstOrDefault();
            if(cart.Quantity > soluongsanpham)
            {
                _notyfService.Warning("Số lượng sản phẩm không đủ, vui lòng nhập lại số lượng");
                return false;
            }
            else
            {
                _db.cart.Update(cart);
                await _db.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> UpdateMultil(Cart cart)
        {
            var cartnew = _db.cart.Where(x => x.Id == cart.Id).FirstOrDefault();
            cartnew.Quantity = cart.Quantity;
            _db.cart.Update(cartnew);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
