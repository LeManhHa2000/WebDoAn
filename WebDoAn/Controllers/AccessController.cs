using Microsoft.AspNetCore.Mvc;
using WebDoAn.dbs;
using WebDoAn.Models;
using WebDoAn.Service.Admin.Users;
using static WebDoAn.Enums.UserEnum;

namespace WebDoAn.Controllers
{
    public class AccessController : Controller
    {
        public readonly DoAnDbContext _db;
        public readonly IUserService _userService;
		public AccessController(DoAnDbContext db, IUserService userService)
        {
            _db = db;
            _userService = userService;
        }
        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.MessageErrorRegister = null;
            ViewBag.MessageSuccessRegister = null;

			if (HttpContext.Session.GetString("PhoneNumber") == null)
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
            if(HttpContext.Session.GetString("PhoneNumber") == null)
            {
                var u = _db.user.Where(x => x.PhoneNumber.Equals(user.PhoneNumber) && x.Password.Equals(user.Password)).FirstOrDefault();
                if(u != null)
                {
                    if (u.Active)
                    {
						HttpContext.Session.SetString("PhoneNumber", u.PhoneNumber.ToString());
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
					ViewBag.MessageErrorLogin = "Số điện thoại hoặc mật khẩu không chính xác!";
				}
            }

			return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("PhoneNumber");
            HttpContext.Session.Remove("Role");
            HttpContext.Session.Remove("UserId");
			return RedirectToAction("Login", "Access");
        }

		public IActionResult Register()
		{
			ViewBag.MessageErrorLogin = null;
			ViewBag.MessageActiveLogin = null;
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(User user)
		{
			var isCreate = await _userService.Create(user);
			if (isCreate)
			{
                ViewBag.MessageSuccessRegister = "Đăng kí thành công !";
				return RedirectToAction("Login", "Access");
			}
			else
			{
				ViewBag.MessageErrorRegister = "Số điện thoại đã được đăng kí! Vui lòng thay đổi.";
				return View(user);
			}
		}
	}
}
