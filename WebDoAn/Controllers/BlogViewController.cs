using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDoAn.dbs;
using WebDoAn.Service.Admin.Blogs;

namespace WebDoAn.Controllers
{
    public class BlogViewController : Controller
    {
        public readonly IBlogService _blogService;
        private readonly DoAnDbContext _db;
        public BlogViewController(IBlogService blogService, DoAnDbContext db)
        {
            _blogService = blogService;
            _db = db;
        }
        public IActionResult Index()
        {
            var listbolg = _blogService.GetAllBogUser();
            return View(listbolg);
        }

        public async Task<IActionResult> Details(int? id)
        {
            var blog = await _blogService.GetBlogById(id.Value);
            if (blog == null)
            {
                return NotFound();
            }
            ViewBag.AuthorName = _db.user.Find(blog.UserId).FullName.ToString();

            return View(blog);
        }
    }
}
