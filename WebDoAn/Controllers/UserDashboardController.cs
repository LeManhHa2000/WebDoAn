using Microsoft.AspNetCore.Mvc;
using WebDoAn.Models;
using WebDoAn.Service.Admin.Users;

namespace WebDoAn.Controllers
{
    public class UserDashboardController : Controller
    {
        public readonly IUserService _userService;
        private readonly IHttpContextAccessor _contxt;
        public UserDashboardController(IUserService userService, IHttpContextAccessor contxt)
        {
            _userService = userService;
            _contxt = contxt;
        }
        public async Task<IActionResult> Index(int id)
        {
            if(_contxt.HttpContext.Session.GetInt32("UserId") != id)
            {
                return NotFound();
            }
            var user = await _userService.GetUserById(id);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(User user)
        {
            var isUpdate = await _userService.UpdateUser(user);
            if (isUpdate)
            {
                return View(user);
            }
            else
            {
                return View(user);

            }
        }
    }
}
