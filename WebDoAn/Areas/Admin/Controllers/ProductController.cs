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
using WebDoAn.Authentications;

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
        [Authentication]
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
            ViewBag.ListImgProDt = _context.productImg.Where(x => x.ProductId == id).Select(x => x.ImgSrc).ToList();

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
                //productview.Photo.CopyTo(new FileStream(filepath, FileMode.Create));

                Product product = new Product{
                    Name = productview.Name,
                    Description = productview.Description,
                    ShortDescription = productview.ShortDescription,
                    //TypeProduct = productview.TypeProduct,
                    Price = productview.Price,
                    Discount = productview.Discount,
                    Length = productview.Length,
                    Height = productview.Height,
                    Width = productview.Width,
                    Material = productview.Material,
                    Evaluate = productview.Evaluate,
                    Quantity = productview.Quantity,
                    CategoryId = productview.CategoryId,
                };
                var isproduct = await _productService.Create(product);
                // Tạo mới sản phẩm thành công
                if (isproduct > 0)
                {
                    foreach(IFormFile photo in productview.Photo)
                    {
                        String uploadfolder = Path.Combine(_hostEnvironment.WebRootPath + "\\images", "product");
                        filename = Guid.NewGuid().ToString() + "_" + photo.FileName;
                        string filepath = Path.Combine(uploadfolder, filename);

                        using (var sream = System.IO.File.Create(filepath))
                        {
                            photo.CopyTo(sream);
                        }

                        ProductImg productImg = new ProductImg
                        {
                            ProductId = isproduct,
                            ImgSrc = filename,
                        };

                        // thêm vào bảng ảnh sản phẩm
                        _context.productImg.Add(productImg);
                        await _context.SaveChangesAsync();
                    }
                    
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["CategoryId"] = new SelectList(_context.categorie, "Id", "Name", productview.CategoryId);
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
            ViewBag.ProImgEdit = _context.productImg.Where(x => x.ProductId == id).ToList();
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

            String filename = "";
            if (!ModelState.IsValid)
            {
                ViewData["ProductId"] = pro.Id;
                ViewData["CategoryId"] = new SelectList(_context.categorie, "Id", "Name", productview.CategoryId);
                return View(productview);

            }

            // Update lai hinh anh
            //if(productview.Photo != null)
            //{
            //    string uploadfolder = Path.Combine(_hostEnvironment.WebRootPath + "\\images", "product");
            //    filename = Guid.NewGuid().ToString() + "_" + productview.Photo.FileName;
            //    string filepath = Path.Combine(uploadfolder, filename);

            //    using (var sream = System.IO.File.Create(filepath))
            //    {
            //        productview.Photo.CopyTo(sream);
            //    }
            //    //productview.Photo.CopyTo(new FileStream(filepath, FileMode.Create));

            //    //delete oldfile
            //    string oldfilename = _hostEnvironment.WebRootPath + "\\images\\product\\" + pro.Image;
            //    System.IO.File.Delete(oldfilename);
            //}

            pro.Name = productview.Name;
            pro.Description = productview.Description;
            pro.ShortDescription = productview.ShortDescription;
            //pro.TypeProduct = productview.TypeProduct;
            pro.Price = productview.Price;
            pro.Discount = productview.Discount;
            pro.Length = productview.Length;
            pro.Width = productview.Width;
            pro.Height = productview.Height;
            pro.Evaluate = productview.Evaluate;
            pro.Material = productview.Material;
            pro.Quantity = productview.Quantity;
            pro.CategoryId = productview.CategoryId;

            var isproduct = await _productService.Update(pro);
            if (isproduct)
            {
                if (productview.Photo != null)
                {
                    foreach (IFormFile photo in productview.Photo)
                    {
                        String uploadfolder = Path.Combine(_hostEnvironment.WebRootPath + "\\images", "product");
                        filename = Guid.NewGuid().ToString() + "_" + photo.FileName;
                        string filepath = Path.Combine(uploadfolder, filename);

                        using (var sream = System.IO.File.Create(filepath))
                        {
                            photo.CopyTo(sream);
                        }

                        ProductImg productImg = new ProductImg
                        {
                            ProductId = pro.Id,
                            ImgSrc = filename,
                        };

                        // thêm vào bảng ảnh sản phẩm
                        _context.productImg.Add(productImg);
                        await _context.SaveChangesAsync();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewData["ProductId"] = pro.Id;
                ViewData["CategoryId"] = new SelectList(_context.categorie, "Id", "Name", productview.CategoryId);
                return View(productview);
            }
            
        }

        // Xóa ảnh trong sản phẩm ( Phần lưu , xóa ProductImg)
        [HttpPost]
        public async Task<ActionResult> DeleteProImg(int id)
        {
            var proImg = _context.productImg.Find(id);
            _context.productImg.Remove(proImg);
            await _context.SaveChangesAsync();
          
            var code = new { Success = true };
            return Json(code);
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
