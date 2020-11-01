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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="sortOrder"></param>
        /// <param name="filterCurrent"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public async Task<IActionResult> Index(string searchString, string sortOrder, string filterCurrent, int? pageNumber)
        {
            // 頁面上 目前排序的值
            ViewData["CurrentSort"] = sortOrder;
            // 頁面上 產品 排序的值   如果等於 NULL ? 倒排序 : 正排序
            ViewData["SortProductName"] = String.IsNullOrEmpty(sortOrder) ? "ProductName_Desc" : "";
            // 頁面上 價錢 排序的值  如果等於 UnitPrice ? 倒排序 : 正排序
            ViewData["SortCompanyName"] = sortOrder == "CompanyName" ? "CompanyName_Desc" : "CompanyName";
            // 頁面上 價錢 排序的值  如果等於 UnitPrice ? 倒排序 : 正排序
            ViewData["SortCategoryName"] = sortOrder == "CategoryName" ? "CategoryName_Desc" : "CategoryName";
            // 頁面上 價錢 排序的值  如果等於 UnitPrice ? 倒排序 : 正排序
            ViewData["SortUnitPrice"] = sortOrder == "UnitPrice" ? "UnitPrice_Desc" : "UnitPrice";

            // 尋找 / 排序 / 上頁、下頁，searchString 用來
            if (searchString != null)
            {
                pageNumber = 1; // 尋找、排序 用 searchString ，頁面歸1。
            }
            else
            {
                // 上頁、下頁 用 currentFilter，searchString=空白
                searchString = filterCurrent; // 等於 filterCurrent
            }
            // 頁面上 目前篩選的值
            ViewData["FilterCurrent"] = searchString;

            // 查詢 LINQ
            var model = from m in _context.Products
                        .Include(s => s.Supplier)
                        .Include(c => c.Category)
                        select m;
            // 如果 查詢文字 不是空值
            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(m => m.ProductName.Contains(searchString)
                || m.Supplier.CompanyName.Contains(searchString)
                || m.Category.CategoryName.Contains(searchString));
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
                case "CompanyName":
                    model = model.OrderBy(m => m.Supplier.CompanyName);
                    break;
                case "CompanyName_Desc":
                    model = model.OrderByDescending(m => m.Supplier.CompanyName);
                    break;
                case "CategoryName":
                    model = model.OrderBy(m => m.Category.CategoryName);
                    break;
                case "CategoryName_Desc":
                    model = model.OrderByDescending(m => m.Category.CategoryName);
                    break;
                default:
                    model = model.OrderBy(m => m.ProductName);
                    break;
            }

            int pageSize = 5;
            // 回傳 PaginatedList.cs 的 PaginatedList
            return View(await PaginatedList<ProductsModel>.CreateAsync(model.AsNoTracking(), pageNumber ?? 1, pageSize));
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
            DdlSupplier();
            DdlCategory();
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

            var productsModel = await _context.Products
                        .Include(s => s.Supplier)
                        .Include(c => c.Category)
                        .FirstOrDefaultAsync(m => m.ProductID == id);
            if (productsModel == null)
            {
                return NotFound();
            }
            DdlSupplier();
            DdlCategory();
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


        #region "下拉式選單"

        /// <summary>
        /// 下拉式選單 Supplier
        /// </summary>
        /// <param name="selected"></param>
        private void DdlSupplier(object selected = null)
        {
            var query = from m in _context.Suppliers
                        orderby m.CompanyName
                        select m;
            ViewBag.SupplierID = new SelectList(query.AsNoTracking(), "SupplierID", "CompanyName", selected);
        }

        /// <summary>
        /// 下拉式選單 Category
        /// </summary>
        /// <param name="selected"></param>
        private void DdlCategory(object selected = null)
        {
            var query = from m in _context.Categories
                        orderby m.CategoryName
                        select m;
            ViewBag.CategoryID = new SelectList(query.AsNoTracking(), "CategoryID", "CategoryName", selected);
        }

        #endregion


        private bool ProductsModelExists(int id)
        {
            return _context.Products.Any(e => e.ProductID == id);
        }
    }
}
