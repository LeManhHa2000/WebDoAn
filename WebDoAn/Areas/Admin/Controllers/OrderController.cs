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
using AspNetCoreHero.ToastNotification.Abstractions;
using WebDoAn.Service.Admin.Orders;
using WebDoAn.Service.Admin.Orders.Dto;
using WebDoAn.ModelPrivew;
using WebDoAn.Authentications;

namespace WebDoAn.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly DoAnDbContext _context;
        public readonly IProductService _productService;
        public INotyfService _notyfService;
        public readonly IOrderService _orderService;
        private readonly IHttpContextAccessor _contxt;

        public OrderController(DoAnDbContext context, IProductService productService, INotyfService notyfService, IOrderService orderService, IHttpContextAccessor contxt)
        {
            _context = context;
            _productService = productService;
            _orderService = orderService;
            _contxt = contxt;
        }

        // GET: Admin/Order
        [Authentication]
        public async Task<IActionResult> Index()
        {
            var doAnDbContext = _context.order.Include(o => o.user);
            return View(await doAnDbContext.ToListAsync());
        }

        public JsonResult GetAllOrder(GetInputByMa input)
        {
            var list = _orderService.GetAllbyAdmin(input);
            return Json(list);
        }

        // GET: Admin/Order/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.order == null)
            {
                return NotFound();
            }
            var order = await _context.order.Where(x => x.Id == id).FirstOrDefaultAsync();

            var user = _context.user.Where(x => x.Id == order.UserId).FirstOrDefault();

            var listorderDetail = _context.orderDetail.Where(x => x.OrderId == id).ToList();
            var listproduct = _context.product.ToList();

            var listodnew = (from a in listorderDetail
                             join b in listproduct on a.ProductId equals b.Id
                             select new OrderDetailViewModal
                             {
                                 Name = b.Name,
                                 Quantity = a.Quantity,
                                 Total = a.Total,
                                 Sum = a.Quantity * a.Total,
                             }).ToList();

            ViewBag.UserOrderView = user;
            ViewBag.ListOrderDetail = listodnew;

            return View(order);
        }

        // GET: Admin/Order/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.user, "Id", "Id");
            return View();
        }

        // POST: Admin/Order/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Code,CreateTime,UpdateTime,ShipDate,Total,Status,PaymentMethod,Note,UserId")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.user, "Id", "Id", order.UserId);
            return View(order);
        }

        // GET: Admin/Order/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.order == null)
            {
                return NotFound();
            }

            var order = await _context.order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            var userorder = _context.user.Find(order.UserId);
            ViewBag.UserOrder = userorder;
            
            return View(order);
        }

        // POST: Admin/Order/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Order order)
        {

            var isUpdate = await _orderService.Update(order);
            if (isUpdate)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(order);

            }
        }

        // GET: Admin/Order/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.order == null)
            {
                return NotFound();
            }

            var order = await _context.order
                .Include(o => o.user)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Admin/Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.order == null)
            {
                return Problem("Entity set 'DoAnDbContext.order'  is null.");
            }
            var order = await _context.order.FindAsync(id);
            if (order != null)
            {
                _context.order.Remove(order);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
          return (_context.order?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
