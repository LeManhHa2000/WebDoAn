using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebDoAn.dbs;
using WebDoAn.Models;

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
            var list = (from t in listproduct
                        orderby t.Id
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
