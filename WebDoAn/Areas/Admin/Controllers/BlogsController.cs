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

namespace WebDoAn.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogsController : Controller
    {
        private readonly DoAnDbContext _context;
        public readonly IBlogService _blogService;

        public BlogsController(DoAnDbContext context, IBlogService blogService)
        {
            _context = context;
            _blogService = blogService;
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

            var blog = await _context.blog
                .Include(b => b.user)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // GET: Admin/Blogs/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.user, "Id", "Id");
            return View();
        }

        // POST: Admin/Blogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogViewModal blog)
        {
            if (ModelState.IsValid)
            {
                _context.Add(blog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.user, "Id", "Id", blog.UserId);
            return View(blog);
        }

        // GET: Admin/Blogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.blog == null)
            {
                return NotFound();
            }

            var blog = await _context.blog.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.user, "Id", "Id", blog.UserId);
            return View(blog);
        }

        // POST: Admin/Blogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CreateTime,Title,Description,SubDescription,ImgSrc,UserId")] Blog blog)
        {
            if (id != blog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(blog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogExists(blog.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.user, "Id", "Id", blog.UserId);
            return View(blog);
        }

        // GET: Admin/Blogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.blog == null)
            {
                return NotFound();
            }

            var blog = await _context.blog
                .Include(b => b.user)
                .FirstOrDefaultAsync(m => m.Id == id);
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
                return Problem("Entity set 'DoAnDbContext.blog'  is null.");
            }
            var blog = await _context.blog.FindAsync(id);
            if (blog != null)
            {
                _context.blog.Remove(blog);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogExists(int id)
        {
          return (_context.blog?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
