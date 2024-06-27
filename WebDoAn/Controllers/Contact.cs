using Microsoft.AspNetCore.Mvc;

namespace WebDoAn.Controllers
{
    public class Contact : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
