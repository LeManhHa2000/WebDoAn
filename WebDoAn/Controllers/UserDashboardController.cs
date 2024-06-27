using Microsoft.AspNetCore.Mvc;

namespace WebDoAn.Controllers
{
    public class UserDashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
