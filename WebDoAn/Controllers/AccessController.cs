using AspNetCoreHero.ToastNotification.Abstractions;
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
		public INotyfService _notyfService;
        private readonly IHttpContextAccessor _contxt;
        public AccessController(DoAnDbContext db, IUserService userService, INotyfService notyfService, IHttpContextAccessor contxt)
        {
            _db = db;
            _userService = userService;
			_notyfService = notyfService;
            _contxt = contxt;
        }
        [HttpGet]
        public IActionResult Login()
        {
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
                        _contxt.HttpContext.Session.SetInt32("Role", ((int)u.Role));
                        _contxt.HttpContext.Session.SetInt32("UserId", u.Id);

						if (u.Role == RoleUser.Admin)
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
						_notyfService.Error("Tài khoản của bạn hiện không được phép đăng nhập! Vui lòng liên hệ với Quản trị viên để biết thông tin");
					}
                }
                else
                {
					_notyfService.Warning("Số điện thoại hoặc mật khẩu không chính xác!");
				}
            }

			return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("PhoneNumber");

            _contxt.HttpContext.Session.Remove("Role");
            _contxt.HttpContext.Session.Remove("UserId");
            
			return RedirectToAction("Login", "Access");
        }

		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(User user)
		{
            var isCreate = await _userService.Create(user);
            if (isCreate)
            {
                return RedirectToAction("Login", "Access");
            }
            else
            {
                return View(user);
            }

        }
	}
}
