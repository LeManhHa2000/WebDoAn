using Microsoft.AspNetCore.Mvc;
using WebDoAn.dbs;
using WebDoAn.Models;
using X.PagedList;

namespace WebDoAn.Controllers
{
    public class ProductViewController : Controller
    {
        private readonly DoAnDbContext _db;
        public ProductViewController(DoAnDbContext db)
        {
            _db = db;
        }

        public IActionResult Index(int id, string? name, int? page)
        {
            //Lay ds danh muc
            var listcate = _db.categorie.ToList();
            ViewBag.Listcate = listcate;

            int pageSize = 6;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            List<Product> products = new List<Product>();

            ViewBag.CateIdSearch = id;
            ViewBag.NameProSeacrh = name;

            if (name == null) 
            {
                if(id == 0)
                {
                    var listproduct = _db.product.ToList();
                    ViewBag.Listproductcount = listproduct.Count;
                    products = listproduct;
                    //ViewBag.Listproduct = listproduct;
                }
                else
                {
                    var listproduct = _db.product.Where(x => x.CategoryId == id).ToList();
                    ViewBag.Listproductcount = listproduct.Count;
                    products = listproduct;
                    //ViewBag.Listproduct = listproduct;
                }
            }
            else
            {
                if (id == 0)
                {
                    var listproduct = _db.product.Where(x => x.Name.ToLower().Contains(name.ToLower())).ToList();
                    ViewBag.Listproductcount = listproduct.Count;
                    products = listproduct;
                    //ViewBag.Listproduct = listproduct;
                }
                else
                {
                    var listproduct = _db.product.Where(x => x.CategoryId == id && x.Name.ToLower().Contains(name.ToLower())).ToList();
                    ViewBag.Listproductcount = listproduct.Count;
                    products = listproduct;
                    //ViewBag.Listproduct = listproduct;
                }
            }

            PagedList<Product> lst = new PagedList<Product>(products, pageNumber, pageSize);
            
            return View(lst);
        }

        public async Task<IActionResult> Details(int? id)
        {
            var product = await _db.product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            var danhmuc = _db.categorie.Find(product.CategoryId).Name;
            ViewBag.DanhMucName = danhmuc;

            var productlienquan = _db.product.Where(x => x.Id != id.Value && x.CategoryId == product.CategoryId).ToList();
            //Sanpham
            var list = (from t in productlienquan
                        orderby t.Id
                        select t).Take(4);
            var listpro = list.ToList();
            ViewBag.ProductLienQuan = listpro;

            return View(product);
        }
    }
}
