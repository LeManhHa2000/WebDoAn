using Microsoft.AspNetCore.Mvc;

namespace WebDoAn.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
