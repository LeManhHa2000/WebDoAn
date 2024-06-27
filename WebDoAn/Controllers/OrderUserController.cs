using Microsoft.AspNetCore.Mvc;

namespace WebDoAn.Controllers
{
    public class OrderUserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateOrderUser()
        {
            return View();
        }
    }
}
