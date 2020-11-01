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
    public class CategoriesController : Controller
    {
        private readonly NorthwindContext _context;

        public CategoriesController(NorthwindContext context)
        {
            _context = context;
        }


        #region "index"

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
        }

        #endregion


        #region "Details"

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoriesModel = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryID == id);
            if (categoriesModel == null)
            {
                return NotFound();
            }

            return View(categoriesModel);
        }

        #endregion


        #region "Create"

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryID,CategoryName,Description,Picture")] CategoriesModel categoriesModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categoriesModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoriesModel);
        }

        #endregion


        #region "Edit"

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoriesModel = await _context.Categories.FindAsync(id);
            if (categoriesModel == null)
            {
                return NotFound();
            }
            return View(categoriesModel);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryID,CategoryName,Description,Picture")] CategoriesModel categoriesModel)
        {
            if (id != categoriesModel.CategoryID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoriesModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriesModelExists(categoriesModel.CategoryID))
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
            return View(categoriesModel);
        }

        #endregion


        #region "Delete"

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoriesModel = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryID == id);
            if (categoriesModel == null)
            {
                return NotFound();
            }

            return View(categoriesModel);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoriesModel = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(categoriesModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        #endregion


        private bool CategoriesModelExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryID == id);
        }
    }
}
