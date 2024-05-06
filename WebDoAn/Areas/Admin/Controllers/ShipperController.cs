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
    public class ShipperController : Controller
    {
        private readonly DoAnDbContext _context;
        public INotyfService _notyfService;

        public ShipperController(DoAnDbContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/Shipper
        public async Task<IActionResult> Index()
        {
              return _context.shipper != null ? 
                          View(await _context.shipper.ToListAsync()) :
                          Problem("Entity set 'DoAnDbContext.shipper'  is null.");
        }

        // GET: Admin/Shipper/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.shipper == null)
            {
                return NotFound();
            }

            var shipper = await _context.shipper
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shipper == null)
            {
                return NotFound();
            }

            return View(shipper);
        }

        // GET: Admin/Shipper/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Shipper/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CreateTime,Name,PhoneNumber")] Shipper shipper)
        {
            var isShipper = _context.shipper.Where(x => x.Name == shipper.Name).ToList();

            if(isShipper.Count == 0)
            {
                if (ModelState.IsValid)
                {
                    var date = DateTime.Now;
                    var datecustom = DateTime.SpecifyKind(date, DateTimeKind.Utc);
                    shipper.CreateTime = datecustom;
                    _context.Add(shipper);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Tạo mới thành công");
                    return RedirectToAction(nameof(Index));
                }
                return View(shipper);
            }
            else
            {
                _notyfService.Error("Người giao hàng này đã tồn tại, vui lòng thay đổi tên !");
                return View(shipper);
            }
            
        }

        // GET: Admin/Shipper/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.shipper == null)
            {
                return NotFound();
            }

            var shipper = await _context.shipper.FindAsync(id);
            if (shipper == null)
            {
                return NotFound();
            }
            return View(shipper);
        }

        // POST: Admin/Shipper/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Shipper shipper)
        {
            DateTime customdate = _context.shipper.Where(x => x.Id == shipper.Id).Select(x => x.CreateTime).FirstOrDefault();


            if (ModelState.IsValid)
            {
                try
                {
                    shipper.CreateTime = customdate;
                    _context.Update(shipper);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Cập nhật thành công");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShipperExists(shipper.Id))
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
            return View(shipper);
        }

        // GET: Admin/Shipper/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.shipper == null)
            {
                return NotFound();
            }

            var shipper = await _context.shipper
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shipper == null)
            {
                return NotFound();
            }

            return View(shipper);
        }

        // POST: Admin/Shipper/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.shipper == null)
            {
                return Problem("Entity set 'DoAnDbContext.shipper'  is null.");
            }
            var shipper = await _context.shipper.FindAsync(id);
            if (shipper != null)
            {
                _context.shipper.Remove(shipper);
            }
            
            await _context.SaveChangesAsync();
            _notyfService.Success("Xóa thành công");
            return RedirectToAction(nameof(Index));
        }

        private bool ShipperExists(int id)
        {
          return (_context.shipper?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
