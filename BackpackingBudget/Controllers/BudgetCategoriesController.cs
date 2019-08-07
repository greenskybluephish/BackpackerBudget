using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BackpackingBudget.Data;
using BackpackingBudget.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace BackpackingBudget.Controllers
{
    public class BudgetCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;
        public BudgetCategoriesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
        // GET: BudgetCategories
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BudgetCategory.Include(b => b.Budget);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: BudgetCategories/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            var currentUser = await GetCurrentUserAsync();
            if (id == null)
            {
                return NotFound();
            }

            var budgetCategory = await _context.BudgetCategory
                .Include(b => b.Budget).Where(b => b.Budget.UserId == currentUser.Id).Include(b => b.BudgetItem)
                .FirstOrDefaultAsync(m => m.BudgetCategoryId == id);
            if (budgetCategory == null)
            {
                return NotFound();
            }

            return View(budgetCategory);
        }

        //// GET: BudgetCategories/Create
        //public IActionResult Create()
        //{
        //    ViewData["BudgetId"] = new SelectList(_context.Budget, "BudgetId", "Name");
        //    return View();
        //}

        //// POST: BudgetCategories/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("BudgetCategoryId,Name,BudgetPerDay,BudgetId")] BudgetCategory budgetCategory)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(budgetCategory);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["BudgetId"] = new SelectList(_context.Budget, "BudgetId", "Name", budgetCategory.BudgetId);
        //    return View(budgetCategory);
        //}

        // GET: BudgetCategories/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var budgetCategory = await _context.BudgetCategory.FindAsync(id);
        //    if (budgetCategory == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["BudgetId"] = new SelectList(_context.Budget, "BudgetId", "Name", budgetCategory.BudgetId);
        //    return View(budgetCategory);
        //}

        //// POST: BudgetCategories/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("BudgetCategoryId,Name,BudgetPerDay,BudgetId")] BudgetCategory budgetCategory)
        //{
        //    if (id != budgetCategory.BudgetCategoryId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(budgetCategory);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!BudgetCategoryExists(budgetCategory.BudgetCategoryId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["BudgetId"] = new SelectList(_context.Budget, "BudgetId", "Name", budgetCategory.BudgetId);
        //    return View(budgetCategory);
        //}

        // GET: BudgetCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var budgetCategory = await _context.BudgetCategory
                .Include(b => b.Budget)
                .FirstOrDefaultAsync(m => m.BudgetCategoryId == id);
            if (budgetCategory == null)
            {
                return NotFound();
            }

            return View(budgetCategory);
        }

        // POST: BudgetCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var budgetCategory = await _context.BudgetCategory.FindAsync(id);
            _context.BudgetCategory.Remove(budgetCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BudgetCategoryExists(int id)
        {
            return _context.BudgetCategory.Any(e => e.BudgetCategoryId == id);
        }
    }
}
