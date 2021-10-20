using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using TEST_ASP_APP.Models;

namespace TEST_ASP_APP.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationContext db;

        public HomeController(ApplicationContext context)
        {
            db = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await db.Stores.Include(u => u.Products).ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Store store)
        {
            db.Stores.Add(store);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> ProductsIndex(int storeId)
        {
            ViewBag.storeId = storeId;
            return View("ProductsIndex", await db.Products.Where(x => x.StoreId == storeId).ToListAsync());
        }

        public IActionResult CreateProduct(int storeId)
        {
            ViewBag.StoreId = storeId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct([Bind("ProductId,Name,Description, StoreId")] Product product, int storeid)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(ProductsIndex), new { storeid = storeid });
            }
            return View(product);
        }

        public async Task<IActionResult> EditProduct(int productid, int storeid)
        {
            ViewBag.StoreId = storeid;
            var product = await db.Products.FindAsync(productid);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct([Bind("ProductId,Name,Description,StoreId")] Product product, int storeid)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(product);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ProductsIndex), new { storeid = storeid });
            }
            return View(product);
        }

        public async Task<IActionResult> DeleteProduct(int productid, int storeid)
        {
            ViewBag.StoreId = storeid;
            var product = await db.Products
                .Include(p => p.Store)
                .FirstOrDefaultAsync(m => m.ProductId == productid);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost, ActionName("DeleteProduct")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProductConfirmed(int productid, int storeid)
        {
            var product = await db.Products.FindAsync(productid);
            db.Products.Remove(product);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(ProductsIndex), new { storeid = storeid });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private bool ProductExists(int id)
        {
            return db.Products.Any(e => e.ProductId == id);
        }
    }
}
