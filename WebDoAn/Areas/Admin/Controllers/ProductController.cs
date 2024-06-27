using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebDoAn.Models;
using WebDoAn.dbs;
using WebDoAn.Service.Admin.Products;
using WebDoAn.Service.Admin.Products.Dto;
using WebDoAn.ModelPrivew;
using static WebDoAn.Enums.ProductEnum;
using System.Xml.Linq;

namespace WebDoAn.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly DoAnDbContext _context;
        private readonly IProductService _productService;
        public IWebHostEnvironment _hostEnvironment;

        public ProductController(DoAnDbContext context, IProductService productService, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _productService = productService;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Admin/Product
        public async Task<IActionResult> Index()
        {
            return View(await _productService.GetProducts());
        }

        // GetAll by input
        public JsonResult GetAllProduct(GetInput input)
        {
            var list = _productService.GetAll(input);
            return Json(list);
        }

        // GET: Admin/Product/Details/5
        // View
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.product == null)
            {
                return NotFound();
            }

            var product = await _productService.GetProductById(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.cateName = await _productService.GetNameCategory(product.CategoryId);

            return View(product);
        }

        // GET: Admin/Product/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.categorie, "Id", "Name");
            return View();
        }

        // POST: Admin/Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModal productview)
        {
            String filename = "";
            if (ModelState.IsValid)
            {
                String uploadfolder = Path.Combine(_hostEnvironment.WebRootPath + "\\images" , "product");
                filename = Guid.NewGuid().ToString() + "_" + productview.Photo.FileName;
                string filepath = Path.Combine(uploadfolder, filename);

                using (var sream = System.IO.File.Create(filepath))
                {
                    productview.Photo.CopyTo(sream);
                }
                //productview.Photo.CopyTo(new FileStream(filepath, FileMode.Create));

                Product product = new Product{
                    Name = productview.Name,
                    Description = productview.Description,
                    ShortDescription = productview.ShortDescription,
                    //TypeProduct = productview.TypeProduct,
                    Price = productview.Price,
                    Length = productview.Length,
                    Height = productview.Height,
                    Width = productview.Width,
                    Material = productview.Material,
                    Evaluate = productview.Evaluate,
                    Quantity = productview.Quantity,
                    CategoryId = productview.CategoryId,
                    Image = filename,
                };
                var isproduct = await _productService.Create(product);
                if (isproduct)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["CategoryId"] = new SelectList(_context.categorie, "Id", "Id", productview.CategoryId);
            return View(productview);
       
        }

        // GET: Admin/Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.product == null)
            {
                return NotFound();
            }

            var product = await _productService.GetProductPriViewById(id.Value);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["Filename"] = _context.product.Find(id.Value).Image.ToString();
            ViewData["CategoryId"] = new SelectList(_context.categorie, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // POST: Admin/Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModal productview)
        {
            var pro = await _productService.GetProductById(productview.Id);
            
            
            if (!ModelState.IsValid)
            {
                ViewData["ProductId"] = pro.Id;
                ViewData["Filename"] = pro.Image;
                ViewData["CategoryId"] = new SelectList(_context.categorie, "Id", "Name", productview.CategoryId);
                return View(productview);

            }

            // Update lai hinh anh
            string filename = pro.Image;
            if(productview.Photo != null)
            {
                string uploadfolder = Path.Combine(_hostEnvironment.WebRootPath + "\\images", "product");
                filename = Guid.NewGuid().ToString() + "_" + productview.Photo.FileName;
                string filepath = Path.Combine(uploadfolder, filename);

                using (var sream = System.IO.File.Create(filepath))
                {
                    productview.Photo.CopyTo(sream);
                }
                //productview.Photo.CopyTo(new FileStream(filepath, FileMode.Create));

                //delete oldfile
                string oldfilename = _hostEnvironment.WebRootPath + "\\images\\product\\" + pro.Image;
                System.IO.File.Delete(oldfilename);
            }

            pro.Name = productview.Name;
            pro.Description = productview.Description;
            pro.ShortDescription = productview.ShortDescription;
            //pro.TypeProduct = productview.TypeProduct;
            pro.Price = productview.Price;
            pro.Length = productview.Length;
            pro.Width = productview.Width;
            pro.Height = productview.Height;
            pro.Evaluate = productview.Evaluate;
            pro.Material = productview.Material;
            pro.Quantity = productview.Quantity;
            pro.CategoryId = productview.CategoryId;
            pro.Image = filename;

            var isproduct = await _productService.Update(pro);
            if (isproduct)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewData["ProductId"] = pro.Id;
                ViewData["Filename"] = pro.Image;
                ViewData["CategoryId"] = new SelectList(_context.categorie, "Id", "Name", productview.CategoryId);
                return View(productview);
            }
            
        }

        // GET: Admin/Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.product == null)
            {
                return NotFound();
            }

            var product = await _productService.GetProductById(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Admin/Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.product == null)
            {
                return Problem("Dữ liệu trống");
            }
            var product = await _productService.GetProductById(id);
            if (product != null)
            {
                await _productService.Delete(id);
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.product?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
