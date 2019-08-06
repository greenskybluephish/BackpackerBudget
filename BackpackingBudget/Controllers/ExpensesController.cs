using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BackpackingBudget.Data;
using BackpackingBudget.Models;

namespace BackpackingBudget.Controllers
{
    public class ExpensesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExpensesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Expenses
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BudgetItem.Include(b => b.BudgetCategory);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Expenses/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Expenses/Create
        public IActionResult AddExpense()
        {
            ViewData["BudgetCategoryId"] = new SelectList(_context.BudgetCategory, "BudgetCategoryId", "Name");
            return View();
        }

        public IActionResult OnGetPartial() =>
            Partial("_AuthorPartialRP");

        // POST: Expenses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BudgetItemId,BudgetCategoryId,Cost,PurchaseDate,Description")] BudgetItem budgetItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(budgetItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BudgetCategoryId"] = new SelectList(_context.BudgetCategory, "BudgetCategoryId", "Name", budgetItem.BudgetCategoryId);
            return View(budgetItem);
        }

        // GET: Expenses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var budgetItem = await _context.BudgetItem.FindAsync(id);
            if (budgetItem == null)
            {
                return NotFound();
            }
            ViewData["BudgetCategoryId"] = new SelectList(_context.BudgetCategory, "BudgetCategoryId", "Name", budgetItem.BudgetCategoryId);
            return View(budgetItem);
        }

        // POST: Expenses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BudgetItemId,BudgetCategoryId,Cost,PurchaseDate,Description")] BudgetItem budgetItem)
        {
            if (id != budgetItem.BudgetItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(budgetItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BudgetItemExists(budgetItem.BudgetItemId))
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
            ViewData["BudgetCategoryId"] = new SelectList(_context.BudgetCategory, "BudgetCategoryId", "Name", budgetItem.BudgetCategoryId);
            return View(budgetItem);
        }

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
            _context.BudgetItem.Remove(budgetItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BudgetItemExists(int id)
        {
            return _context.BudgetItem.Any(e => e.BudgetItemId == id);
        }
    }
}
