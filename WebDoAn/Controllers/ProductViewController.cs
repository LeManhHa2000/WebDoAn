using Microsoft.AspNetCore.Mvc;
using WebDoAn.dbs;
using WebDoAn.Models;
using WebDoAn.Service.Admin.Products;
using WebDoAn.Service.Admin.Products.Dto;
using X.PagedList;

namespace WebDoAn.Controllers
{
    public class ProductViewController : Controller
    {
        private readonly DoAnDbContext _db;
        private readonly IProductService _productService;
        public ProductViewController(DoAnDbContext db, IProductService productService)
        {
            _db = db;
            _productService = productService;
        }

        public IActionResult Index(int id, string? name, int? page)
        {
            //Lay ds danh muc
            var listcate = _db.categorie.ToList();
            ViewBag.Listcate = listcate;

            int pageSize = 6;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            List<ProductDto> products = new List<ProductDto>();

            ViewBag.CateIdSearch = id;
            ViewBag.NameProSeacrh = name;

            if (name == null) 
            {
                if(id == 0)
                {
                    var listproduct = _db.product.ToList();
                    ViewBag.Listproductcount = listproduct.Count;
                    var listquery = _productService.GetAllProDto(listproduct);
                    products = listquery;
                    //ViewBag.Listproduct = listproduct;
                }
                else
                {
                    var listproduct = _db.product.Where(x => x.CategoryId == id).ToList();
                    ViewBag.Listproductcount = listproduct.Count;
                    var listquery = _productService.GetAllProDto(listproduct);
                    products = listquery;
                    //ViewBag.Listproduct = listproduct;
                }
            }
            else
            {
                if (id == 0)
                {
                    var listproduct = _db.product.Where(x => x.Name.ToLower().Contains(name.ToLower())).ToList();
                    ViewBag.Listproductcount = listproduct.Count;
                    var listquery = _productService.GetAllProDto(listproduct);
                    products = listquery;
                    //ViewBag.Listproduct = listproduct;
                }
                else
                {
                    var listproduct = _db.product.Where(x => x.CategoryId == id && x.Name.ToLower().Contains(name.ToLower())).ToList();
                    ViewBag.Listproductcount = listproduct.Count;
                    var listquery = _productService.GetAllProDto(listproduct);
                    products = listquery;
                    //ViewBag.Listproduct = listproduct;
                }
            }

            PagedList<ProductDto> lst = new PagedList<ProductDto>(products, pageNumber, pageSize);
            
            return View(lst);
        }

        public async Task<IActionResult> Details(int? id)
        {
            var product = await _db.product.FindAsync(id);
            var proresult = _productService.GetProDto(product);
            if (product == null)
            {
                return NotFound();
            }
            var danhmuc = _db.categorie.Find(product.CategoryId).Name;
            ViewBag.DanhMucName = danhmuc;

            var productlienquan = _db.product.Where(x => x.Id != id.Value && x.CategoryId == product.CategoryId).ToList();

            var listresult = _productService.GetAllProDto(productlienquan);
            //Sanpham
            var list = (from t in listresult
                        orderby t.Id
                        select t).Take(4);
            var listpro = list.ToList();
            ViewBag.ProductLienQuan = listpro;

            var listproimg = _db.productImg.Where(x => x.ProductId == id).ToList();
            ViewBag.ListProImgDt = listproimg;

            return View(proresult);
        }
    }
}
