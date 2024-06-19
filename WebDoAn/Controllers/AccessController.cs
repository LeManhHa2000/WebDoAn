using Microsoft.AspNetCore.Mvc;
using WebDoAn.dbs;
using WebDoAn.Models;

namespace WebDoAn.Controllers
{
    public class AccessController : Controller
    {
        public readonly DoAnDbContext _db;
        public AccessController(DoAnDbContext db)
        {
            _db = db;
        }
        [HttpGet]
        public IActionResult Login()
        {
            if(HttpContext.Session.GetString("UserName") == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            if(HttpContext.Session.GetString("UserName") == null)
            {
                var u = _db.user.Where(x => x.UserName.Equals(user.UserName) && x.Password.Equals(user.Password)).FirstOrDefault();
                if(u != null)
                {
                    HttpContext.Session.SetString("UserName", u.UserName.ToString());
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("UserName");
            return RedirectToAction("Login", "Access");
        }
    }
}
