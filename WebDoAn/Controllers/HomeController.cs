using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebDoAn.dbs;
using WebDoAn.Models;
using WebDoAn.Service.Admin.Products.Dto;

namespace WebDoAn.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DoAnDbContext _db;

        public HomeController(ILogger<HomeController> logger, DoAnDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            //Sanpham
            var listproduct = _db.product.ToList();

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

            var list = (from t in listquery
                        orderby t.Id descending
                        select t).Take(8);
            var listpro = list.ToList();
            ViewBag.ProductInHome = listpro;

            //Tintuc
            var listblog = _db.blog.ToList();
            var listb = (from t in listblog
                         orderby t.Id
                        select t).Take(3);
            var listnewblog = listb.ToList();
            ViewBag.BlogInHome = listnewblog;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
