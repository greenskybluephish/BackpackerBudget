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
    public class ExpensesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ExpensesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
        // GET: Expenses
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BudgetItem.Include(b => b.BudgetCategory);
            return View(await applicationDbContext.ToListAsync());
        }

        //// GET: Expenses/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var budgetItem = await _context.BudgetItem
        //        .Include(b => b.BudgetCategory)
        //        .FirstOrDefaultAsync(m => m.BudgetItemId == id);
        //    if (budgetItem == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(budgetItem);
        //}

        // GET: Expenses/Create
        //public IActionResult AddExpense()
        //{


        //    ViewData["BudgetCategoryId"] = new SelectList(_context.BudgetCategory, "BudgetCategoryId", "Name");
        //    return View();
        //}
        [Authorize]
        public async Task<IActionResult> AddExpenseAsync()
        {
            var currentUser = await GetCurrentUserAsync();
            var budgetCategories = await _context.BudgetCategory
    .Where(b => b.Budget.User == currentUser && b.Budget.IsActive).ToListAsync();

            ViewData["BudgetCategoryId"] = new SelectList(budgetCategories, "BudgetCategoryId", "Name");

            return PartialView("AddExpense");
        }
            

        // POST: Expenses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("BudgetItemId,BudgetCategoryId,Cost,PurchaseDate,Description")] BudgetItem budgetItem)
        {
            ModelState.Remove("BudgetItemId");
            ModelState.Remove("PurchaseDate");

            if (ModelState.IsValid)
            {
                budgetItem.PurchaseDate = DateTime.Now;
                _context.Add(budgetItem);
                await _context.SaveChangesAsync();
                return RedirectToAction("Dashboard", "Home");
            }
            var currentUser = await GetCurrentUserAsync();
            var budgetCategories = await _context.BudgetCategory
    .Where(b => b.Budget.User == currentUser && b.Budget.IsActive).ToListAsync();

            ViewData["BudgetCategoryId"] = new SelectList(budgetCategories, "BudgetCategoryId", "Name", budgetItem.BudgetCategoryId);

            return View(budgetItem);
        }

        // GET: Expenses/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var budgetItem = await _context.BudgetItem.FindAsync(id);
        //    if (budgetItem == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["BudgetCategoryId"] = new SelectList(_context.BudgetCategory, "BudgetCategoryId", "Name", budgetItem.BudgetCategoryId);
        //    return View(budgetItem);
        //}

        //// POST: Expenses/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("BudgetItemId,BudgetCategoryId,Cost,PurchaseDate,Description")] BudgetItem budgetItem)
        //{
        //    if (id != budgetItem.BudgetItemId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(budgetItem);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!BudgetItemExists(budgetItem.BudgetItemId))
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
        //    ViewData["BudgetCategoryId"] = new SelectList(_context.BudgetCategory, "BudgetCategoryId", "Name", budgetItem.BudgetCategoryId);
        //    return View(budgetItem);
        //}

        // GET: Expenses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var budgetItem = await _context.BudgetItem
                .Include(b => b.BudgetCategory)
                .FirstOrDefaultAsync(m => m.BudgetItemId == id);
            if (budgetItem == null)
            {
                return NotFound();
            }

            return View(budgetItem);
        }

        // POST: Expenses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var budgetItem = await _context.BudgetItem.FindAsync(id);
            var bcId = budgetItem.BudgetCategoryId;
            _context.BudgetItem.Remove(budgetItem);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "BudgetCategories", new {id = bcId });
        }

        private bool BudgetItemExists(int id)
        {
            return _context.BudgetItem.Any(e => e.BudgetItemId == id);
        }
    }
}
