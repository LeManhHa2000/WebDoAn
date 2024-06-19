using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebDoAn.Models;
using WebDoAn.dbs;
using WebDoAn.Service.Admin.Blogs;
using WebDoAn.Service.Admin.Blogs.Dto;
using WebDoAn.ModelPrivew;
using Microsoft.Extensions.Hosting;

namespace WebDoAn.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogsController : Controller
    {
        private readonly DoAnDbContext _context;
        public readonly IBlogService _blogService;
        public IWebHostEnvironment _hostEnvironment;

        public BlogsController(DoAnDbContext context, IBlogService blogService, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _blogService = blogService;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Admin/Blogs
        public async Task<IActionResult> Index()
        {
            return View(await _blogService.GetBlogs());
        }

        // GetAll category
        public JsonResult GetAllBlog()
        {
            var list = _context.blog.ToList();
            return Json(list);
        }

        public JsonResult GetAllBlogs(GetInput input)
        {
            var list = _blogService.GetAll(input);
            return Json(list);
        }

        // GET: Admin/Blogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.blog == null)
            {
                return NotFound();
            }

            var blog = await _blogService.GetBlogById(id.Value);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // GET: Admin/Blogs/Create
        public IActionResult Create()
        {
            //ViewData["UserId"] = new SelectList(_context.user, "Id", "Id");
            return View();
        }

        // POST: Admin/Blogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogViewModal blogview)
        {
            string filename = "";
            if (ModelState.IsValid)
            {
                String uploadfolder = Path.Combine(_hostEnvironment.WebRootPath + "\\images", "blog");
                filename = Guid.NewGuid().ToString() + "_" + blogview.Photo.FileName;
                string filepath = Path.Combine(uploadfolder, filename);
                using(var sream = System.IO.File.Create(filepath))
                {
                    blogview.Photo.CopyTo(sream);
                }
                //blogview.Photo.CopyTo(new FileStream(filepath, FileMode.Create));

                Blog blognew = new Blog
                {
                    Title = blogview.Title,
                    SubDescription = blogview.SubDescription,
                    Description = blogview.Description,
                    UserId = 1,
                    ImgSrc = filename
                };


                await _blogService.Create(blognew);
                return RedirectToAction(nameof(Index));
            }
            return View(blogview);
        }

        // GET: Admin/Blogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.blog == null)
            {
                return NotFound();
            }

            var blog = await _blogService.GetBlogViewById(id.Value);
            if (blog == null)
            {
                return NotFound();
            }
            ViewData["ImgSrc"] = _context.blog.Find(id).ImgSrc.ToString();
            //ViewData["UserId"] = new SelectList(_context.user, "Id", "Id", blog.UserId);
            return View(blog);
        }

        // POST: Admin/Blogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BlogViewModal blogview)
        {
            var blog = await _blogService.GetBlogById(blogview.Id);
            if (!ModelState.IsValid)
            {
                ViewData["BlogId"] = blog.Id;
                ViewData["ImgSrc"] = blog.ImgSrc;
                return View(blogview);
                
            }

            // Update lai hinh anh
            string filename = blog.ImgSrc;
            if (blogview.Photo != null)
            {
                string uploadfolder = Path.Combine(_hostEnvironment.WebRootPath + "\\images", "blog");
                filename = Guid.NewGuid().ToString() + "_" + blogview.Photo.FileName;
                string filepath = Path.Combine(uploadfolder, filename);
                using (var sream = System.IO.File.Create(filepath))
                {
                    blogview.Photo.CopyTo(sream);
                }
                //blogview.Photo.CopyTo(new FileStream(filepath, FileMode.Create));

                //delete oldfile
                string oldfilenameblog = _hostEnvironment.WebRootPath + "\\images\\blog\\" + blog.ImgSrc;
                System.IO.File.Delete(oldfilenameblog);
            }

            blog.Title = blogview.Title;
            blog.Description = blogview.Description;
            blog.SubDescription = blogview.SubDescription;
            blog.UserId = blog.UserId;
            blog.ImgSrc = filename;

            var isblog = await _blogService.Update(blog);
            if (isblog)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewData["BlogId"] = blog.Id;
                ViewData["ImgSrc"] = blog.ImgSrc;
                return View(blogview);
            }
            //ViewData["UserId"] = new SelectList(_context.user, "Id", "Id", blog.UserId);
        }

        // GET: Admin/Blogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.blog == null)
            {
                return NotFound();
            }

            var blog = await _blogService.GetBlogById(id.Value);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // POST: Admin/Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.blog == null)
            {
                return Problem("Dữ liệu trống");
            }
            var blog = await _blogService.GetBlogById(id);
            if (blog != null)
            {
                await _blogService.Delete(id);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool BlogExists(int id)
        {
          return (_context.blog?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
