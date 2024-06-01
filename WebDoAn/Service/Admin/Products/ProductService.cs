using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.EntityFrameworkCore;
using WebDoAn.dbs;
using WebDoAn.Models;
using WebDoAn.Service.Admin.Products.Dto;

namespace WebDoAn.Service.Admin.Products
{
    public class ProductService : IProductService
    {
        private readonly DoAnDbContext _db;
        public INotyfService _notyfService;

        public ProductService(DoAnDbContext db, INotyfService notyfService) {  
            _db = db; 
            _notyfService = notyfService;
        }
        public async Task<bool> Create(Product product)
        {
            var isProduct = _db.product.Where(x => x.Name.ToLower() == product.Name.ToLower()).ToList();
            if(isProduct.Count() == 0)
            {
                _db.product.Add(product);
                await _db.SaveChangesAsync();
                _notyfService.Success("Tạo mới thành công");
                return true;
            }
            else
            {
                _notyfService.Error("Tên sản phẩm này đã tồn tại, vui lòng thay đổi tên !");
                return false;
            }
        }

        public async Task<bool> Delete(Product id)
        {
            var product = _db.product.Find(id);
            if (product == null)
            {
                _notyfService.Error("Không tìm thấy sản phẩm này");
                return false;
            }
            else
            {
                _db.product.Remove(product);
                 await _db.SaveChangesAsync();
                _notyfService.Success("Xóa thành công");
                return true;
            }
        }

        public List<Product> GetAll(GetInput input)
        {
            if (input.Name == "all")
            {
                var list = _db.product.OrderBy(x => x.Id).ToList();
                return list;
            }
            else
            {
                var list = _db.product.Where(x => x.Name.ToLower().Contains(input.Name.ToLower())).OrderBy(x => x.Id).ToList();
                return list;
            }
        }

        public async Task<Product> GetProductById(int id)
        {
            var prod = await _db.product.FindAsync(id);
            if (prod == null)
            {
                Product product = new Product();
                _notyfService.Error("Không tìm thấy sản phẩm này");
                return product;
            }
            else
            {
                return prod;
            }
        }

        public async Task<List<Product>> GetProducts()
        {
            var listProduct = await _db.product.OrderBy(x => x.Id).ToListAsync();
            return listProduct;
        }

        public async Task<bool> Update(Product product)
        {
            var isProduct = _db.product.Where(x => x.Name.ToLower() == product.Name.ToLower()).ToList();
            if(isProduct.Count() == 0)
            {
                DateTime pro = _db.product.Where(x => x.Id == product.Id).Select(x => x.CreateTime).FirstOrDefault();
                product.CreateTime = pro;
                _db.product.Update(product);
                await _db.SaveChangesAsync();
                _notyfService.Success("Cập nhật thành công");
                return true;
            }
            else
            {
                _notyfService.Error("Tên sản phẩm này đã tồn tại, vui lòng thay đổi tên !");
                return false;
            }
            
        }
    }
}
