using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.EntityFrameworkCore;
using WebDoAn.dbs;
using WebDoAn.ModelPrivew;
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
        public async Task<int> Create(Product product)
        {
            var isProduct = _db.product.Where(x => x.Name.ToLower() == product.Name.ToLower()).ToList();
            if(isProduct.Count() == 0)
            {
                // Tao moi ngay
                var date = DateTime.Now;
                var datecustom = DateTime.SpecifyKind(date, DateTimeKind.Utc);
                product.CreateTime = datecustom;

                _db.product.Add(product);
                await _db.SaveChangesAsync();
                _notyfService.Success("Tạo mới thành công");
                return product.Id;
            }
            else
            {
                _notyfService.Error("Tên sản phẩm này đã tồn tại, vui lòng thay đổi tên !");
                return 0;
            }
        }

        public async Task<bool> Delete(int id)
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

        public List<ProductDto> GetAll(GetInput input)
        {
            if (input.Name == "all")
            {
                var listproduct = _db.product.OrderBy(x => x.Id).ToList();
                var listimg = _db.productImg.ToList();

                var listquery = (from pro in listproduct
                                 select new ProductDto
                                 {
                                     Id = pro.Id,
                                     CreateTime = pro.CreateTime,
                                     UpdateTime = pro.UpdateTime,
                                     Name = pro.Name,
                                     Quantity = pro.Quantity,
                                     Image = _db.productImg.Where(x => x.ProductId == pro.Id).Count() > 0 ? _db.productImg.Where(x => x.ProductId == pro.Id).ToList()[0].ImgSrc : "anhtrong",
                                 }).ToList();
                return listquery;
            }
            else
            {
                var listproduct = _db.product.Where(x => x.Name.ToLower().Contains(input.Name.ToLower())).OrderBy(x => x.Id).ToList();
                var listimg = _db.productImg.ToList();

                var listquery = (from pro in listproduct
                                 select new ProductDto
                                 {
                                     Id = pro.Id,
                                     CreateTime = pro.CreateTime,
                                     UpdateTime = pro.UpdateTime,
                                     Name = pro.Name,
                                     Quantity = pro.Quantity,
                                     Image = (from i in listimg where i.ProductId == pro.Id select i.ImgSrc).ToList()[0],
                                 }).ToList();
                return listquery;
            }
        }

        public List<ProductDto> GetAllProDto(List<Product> listproduct)
        {
            var listquery = (from pro in listproduct
                             select new ProductDto
                             {
                                 Id = pro.Id,
                                 CreateTime = pro.CreateTime,
                                 UpdateTime = pro.UpdateTime,
                                 Price = pro.Price,
                                 Discount = pro.Discount,
                                 Name = pro.Name,
                                 Quantity = pro.Quantity,
                                 Image = _db.productImg.Where(x => x.ProductId == pro.Id).Count() > 0 ? _db.productImg.Where(x => x.ProductId == pro.Id).ToList()[0].ImgSrc : "anhtrong",
                             }).ToList();

            return listquery;
        }

        public ProductDto GetProDto(Product product)
        {
            var proquery = new ProductDto
            {
                Id = product.Id,
                CreateTime = product.CreateTime,
                UpdateTime = product.UpdateTime,
                Price = product.Price,
                Discount = product.Discount,
                Name = product.Name,
                Quantity = product.Quantity,
                Height = product.Height,
                Width = product.Width,
                Length = product.Length,
                Material = product.Material,
                Evaluate = product.Evaluate,
                Description = product.Description,
                ShortDescription = product.ShortDescription,
                Image = _db.productImg.Where(x => x.ProductId == product.Id).Count() > 0 ? _db.productImg.Where(x => x.ProductId == product.Id).ToList()[0].ImgSrc : "anhtrong",
            };

            return proquery;
        }

        public async Task<string> GetNameCategory(int cateId)
        {
            var cate = _db.categorie.Find(cateId);
            if(cate != null)
            {
                return cate.Name.ToString();
            }
            else
            {
                return string.Empty;
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

        public async Task<ProductViewModal> GetProductPriViewById(int id)
        {
            var prod = await _db.product.FindAsync(id);
            if (prod == null)
            {
                ProductViewModal product = new ProductViewModal();
                _notyfService.Error("Không tìm thấy sản phẩm này");
                return product;
            }
            else
            {
                ProductViewModal pro = new ProductViewModal
                {
                    Id = prod.Id,
                    Name = prod.Name,
                    ShortDescription = prod.ShortDescription,
                    Description = prod.Description,
                    CategoryId = prod.CategoryId,
                    Quantity = prod.Quantity,
                    Price = prod.Price,
                    Image = prod.Image,
                    Length = prod.Length,
                    Height = prod.Height,
                    Width = prod.Width,
                    Material = prod.Material,
                    Evaluate = prod.Evaluate,
                };

                return pro;
            }
        }

        public async Task<List<Product>> GetProducts()
        {
            var listProduct = await _db.product.OrderBy(x => x.Id).ToListAsync();
            return listProduct;
        }

        public async Task<bool> Update(Product product)
        {
            var isProduct = _db.product.Where(x => product.Id != x.Id && x.Name.ToLower() == product.Name.ToLower()).ToList();
            if(isProduct.Count() == 0)
            {
                DateTime pro = _db.product.Where(x => x.Id == product.Id).Select(x => x.CreateTime).FirstOrDefault();
                product.CreateTime = pro;

                // Tao moi ngay cập nhật
                var date = DateTime.Now;
                var datecustom = DateTime.SpecifyKind(date, DateTimeKind.Utc);
                product.UpdateTime = datecustom;

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

        public async Task<bool> UpdateQuantity(Product product)
        {
            var productOld = _db.product.Where(x => x.Id == product.Id).FirstOrDefault();
            productOld.Quantity = product.Quantity;
            //product.Width = productOld.Width;
            //product.Height = productOld.Height;
            //product.Length = productOld.Length;
            //product.Name = productOld.Name;
            //product.Price = productOld.Price;
            //product.Description = productOld.Description;
            //product.ShortDescription = productOld.ShortDescription;
            //product.Image = productOld.Image;
            //product.CreateTime = product.CreateTime;
            //product.Material = productOld.Material;
            //product.Evaluate = productOld.Evaluate;
            //product.CategoryId = productOld.CategoryId;

            // Tao moi ngay cập nhật
            var date = DateTime.Now;
            var datecustom = DateTime.SpecifyKind(date, DateTimeKind.Utc);
            productOld.UpdateTime = datecustom;

            _db.product.Update(productOld);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
