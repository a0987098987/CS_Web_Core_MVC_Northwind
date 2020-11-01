using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CS_Web_Core_MVC_Northwind.Data;
using CS_Web_Core_MVC_Northwind.Models;

namespace CS_Web_Core_MVC_Northwind.Controllers
{
    public class ProductsController : Controller
    {
        private readonly NorthwindContext _context;

        public ProductsController(NorthwindContext context)
        {
            _context = context;
        }


        #region "index"

        // GET: Products
        public async Task<IActionResult> Index(string searchString, string sortOrder)
        {
            // 頁面上 ProductName 排序的值     如果等於 NULL ? 倒排序 : 正排序
            ViewData["SortProductName"] = String.IsNullOrEmpty(sortOrder) ? "ProductName_Desc" : "";
            // 頁面上 UnitPrice 排序的值 如果等於 UnitPrice ? 倒排序 : 正排序
            ViewData["SortUnitPrice"] = sortOrder == "UnitPrice" ? "UnitPrice_Desc" : "UnitPrice";
            // 頁面上 現在篩選
            ViewData["searchString"] = searchString;

            // 查詢 LINQ
            var model = from m in _context.Products
                        select m;
            // 如果 查詢文字 不是空值
            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(m => m.ProductName.Contains(searchString));
            }
            // LINQ 排序
            switch (sortOrder)
            {
                case "ProductName_Desc":
                    model = model.OrderByDescending(m => m.ProductName);
                    break;
                case "UnitPrice":
                    model = model.OrderBy(m => m.UnitPrice);
                    break;
                case "UnitPrice_Desc":
                    model = model.OrderByDescending(m => m.UnitPrice);
                    break;
                default:
                    model = model.OrderBy(m => m.ProductName);
                    break;
            }

            return View(await model.ToListAsync());

        }

        #endregion


        #region "Details"

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productsModel = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (productsModel == null)
            {
                return NotFound();
            }

            return View(productsModel);
        }

        #endregion


        #region "Create"

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductID,ProductName,SupplierID,CategoryID,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued")] ProductsModel productsModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productsModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productsModel);
        }

        #endregion


        #region "Edit"

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productsModel = await _context.Products.FindAsync(id);
            if (productsModel == null)
            {
                return NotFound();
            }
            return View(productsModel);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductID,ProductName,SupplierID,CategoryID,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued")] ProductsModel productsModel)
        {
            if (id != productsModel.ProductID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productsModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsModelExists(productsModel.ProductID))
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
            return View(productsModel);
        }

        #endregion


        #region "Delete"

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productsModel = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (productsModel == null)
            {
                return NotFound();
            }

            return View(productsModel);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productsModel = await _context.Products.FindAsync(id);
            _context.Products.Remove(productsModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        #endregion


        private bool ProductsModelExists(int id)
        {
            return _context.Products.Any(e => e.ProductID == id);
        }
    }
}
