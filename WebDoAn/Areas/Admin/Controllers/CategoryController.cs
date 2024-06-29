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
using WebDoAn.Service.Admin.Categories;
using WebDoAn.Service.Admin.Categories.Dto;
using WebDoAn.Authentications;

namespace WebDoAn.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly DoAnDbContext _context;
        private readonly ICategoryService _categoryService;
        public INotyfService _notyfService;

        public CategoryController(DoAnDbContext context, ICategoryService categoryService, INotyfService notyfService)
        {
            _context = context;
            _categoryService = categoryService;
            _notyfService = notyfService;
        }

        // GET: Admin/Category
        [Authentication]
        public async Task<IActionResult> Index()
        {
              return View(await _categoryService.GetCategories());
        }

        // GetAll category
        public JsonResult GetAllCategory()
        {
            var list = _context.categorie.ToList();
            return Json(list);
        }

        public JsonResult GetAlltestCategory(GetInput input)
        {
            var list = _categoryService.GetAll(input);
            return Json(list);
        }

        // GET: Admin/Category/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.categorie == null)
            {
                return NotFound();
            }

            var category = await _categoryService.GetCategoryById(id.Value);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Admin/Category/Create
        // View
        public IActionResult Create()
        {
            return PartialView();
        }

        // POST: Admin/Category/Create
        // Action tạo mới
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            var isCreate = await _categoryService.Create(category);
            if(isCreate)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(category);
            }
        }

        // GET: Admin/Category/Edit/5
        // View
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.categorie == null)
            {
                return NotFound();
            }

            var category = await _categoryService.GetCategoryById(id.Value);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Admin/Category/Edit/5
        // Action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category category)
        {
            var isUpdate = await _categoryService.Update(category);
            if(isUpdate)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(category);

            }

        }

        // GET: Admin/Category/Delete/5
        // View
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.categorie == null)
            {
                return NotFound();
            }

            var category = await _categoryService.GetCategoryById(id.Value);
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
                return Problem("Dữ liệu trống.");
            }
            var category = await _categoryService.GetCategoryById(id);
            if (category != null)
            {
                await _categoryService.Delete(id);
            }
          
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
          return (_context.categorie?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
