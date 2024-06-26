using Microsoft.AspNetCore.Mvc;
using WebDoAn.dbs;
using WebDoAn.Models;
using static WebDoAn.Enums.UserEnum;

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
                    if (u.Active)
                    {
						HttpContext.Session.SetString("UserName", u.UserName.ToString());
						HttpContext.Session.SetInt32("Role", ((int)u.Role));
						HttpContext.Session.SetInt32("UserId", u.Id);
						ViewBag.MessageErrorLogin = null;
						ViewBag.MessageActiveLogin = null;
                        if(u.Role == RoleUser.Admin)
                        {
							return RedirectToAction("Index", "Home", new {Area = "Admin" });
						}
                        else
                        {
							return RedirectToAction("Index", "Home");
						}
						
					}
                    else
                    {
                        ViewBag.MessageErrorLogin = null;
						ViewBag.MessageActiveLogin = "Tài khoản của bạn hiện không được phép đăng nhập! Vui lòng liên hệ với Quản trị viên để biết thông tin";
					}
                }
                else
                {
                    ViewBag.MessageActiveLogin = null;
					ViewBag.MessageErrorLogin = "Tài khoản hoặc mật khẩu không chính xác!";
				}
            }

			return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("UserName");
            HttpContext.Session.Remove("Role");
            HttpContext.Session.Remove("UserId");
			return RedirectToAction("Login", "Access");
        }
    }
}
