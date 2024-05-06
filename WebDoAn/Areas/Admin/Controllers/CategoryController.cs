using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebDoAn.Models;
using WebDoAn.dbs;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace WebDoAn.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly DoAnDbContext _context;
        public INotyfService _notyfService;

        public CategoryController(DoAnDbContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/Category
        public async Task<IActionResult> Index()
        {
              return _context.categorie != null ? 
                          View(await _context.categorie.OrderBy(x => x.Id).ToListAsync()) :
                          Problem("Entity set 'DoAnDbContext.categorie'  is null.");
        }

        // GET: Admin/Category/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.categorie == null)
            {
                return NotFound();
            }

            var category = await _context.categorie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Admin/Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CreateTime,Name,Description,Status")] Category category)
        {
            var isCate = _context.categorie.Where(x => x.Name == category.Name).ToList();
            if (isCate.Count == 0)
            {

                if (ModelState.IsValid)
                {
                    var date = DateTime.Now;
                    var datecustom = DateTime.SpecifyKind(date, DateTimeKind.Utc);
                    category.CreateTime = datecustom;
                    _context.Add(category);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Tạo mới thành công");
                    return RedirectToAction(nameof(Index));
                }
                return View(category);
            }
            else
            {
                _notyfService.Error("Tên loại hàng này đã tồn tại, vui lòng thay đổi tên !");
                return View(category);
            }
        }

        // GET: Admin/Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.categorie == null)
            {
                return NotFound();
            }

            var category = await _context.categorie.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Admin/Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category category)
        {
            DateTime cate = _context.categorie.Where(x => x.Id == category.Id).Select(x => x.CreateTime).FirstOrDefault();

            if (ModelState.IsValid)
            {
                try
                {
                    category.CreateTime = cate;
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Cập nhật thành công");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            return View(category);
        }

        // GET: Admin/Category/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.categorie == null)
            {
                return NotFound();
            }

            var category = await _context.categorie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Admin/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.categorie == null)
            {
                return Problem("Entity set 'DoAnDbContext.categorie'  is null.");
            }
            var category = await _context.categorie.FindAsync(id);
            if (category != null)
            {
                _context.categorie.Remove(category);
            }
            
            await _context.SaveChangesAsync();
            _notyfService.Success("Xóa thành công");
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
          return (_context.categorie?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
