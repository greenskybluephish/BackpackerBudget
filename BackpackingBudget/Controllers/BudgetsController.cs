﻿using System;
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
using BackpackingBudget.Models.ViewModels;

namespace BackpackingBudget.Controllers
{
    public class BudgetsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;
        public BudgetsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: Budgets
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var currentUser = await GetCurrentUserAsync();
            var budgets = await _context.Budget.Include(b => b.User).Where(b => b.User == currentUser).ToListAsync();
            if (budgets != null)
            {
                return View(budgets);
            }
            else
            {
                List<Budget> empty = new List<Budget>();
                return View(empty);
            }
        }

        // GET: Budgets/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var currentUser = await GetCurrentUserAsync();
            var budget = await _context.Budget.Include(b => b.BudgetCategory).FirstOrDefaultAsync(m => m.BudgetId == id && m.UserId == currentUser.Id);

            return View(budget);

        }


        // GET: Budgets/Create
        public IActionResult Create()
        {

            ViewBag.minDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 7);
            ViewBag.data = new string[] { "Transportation", "Lodging", "Food", "Activities", "Misc", "Non-Daily Expenses (Untracked)" };
            return View();
        }


        // POST: Budgets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(string [] categories, [Bind("Name,UserId,StartDate,EndDate,BudgetAmount,IsActive")] Budget budget)
        {
            if (categories.Length == 0 )
            {
                return View(budget);
            }

            ModelState.Remove("User");
            ModelState.Remove("UserId");
            if (ModelState.IsValid)
            {
                var currentUser = await GetCurrentUserAsync();
                budget.UserId = currentUser.Id;


                if (budget.IsActive)
                {
                    var makeInactive = await _context.Budget.Where(b => b.UserId == budget.UserId && b.IsActive).FirstOrDefaultAsync();
                    if (makeInactive != null)
                    {
                        makeInactive.IsActive = false;
                        _context.Update(makeInactive);
                        await _context.SaveChangesAsync();
                    }

                }
                _context.Add(budget);
                await _context.SaveChangesAsync();

                var postedBudget = await _context.Budget.Where(b => b == budget).FirstOrDefaultAsync();

                foreach (string category in categories)
                {
                    BudgetCategory bc = new BudgetCategory()
                    {
                        Name = category,
                        BudgetId = postedBudget.BudgetId,
                        BudgetPerDay = 0,
                    };
                    _context.BudgetCategory.Add(bc);
                }
                await _context.SaveChangesAsync();

                BudgetViewModel model = new BudgetViewModel();
                model.Budget = postedBudget;
                model.BudgetCategories = await _context.BudgetCategory.Where(bc => bc.BudgetId == budget.BudgetId).ToListAsync();

                return View("CreateDetails", model);
            }

            return View(budget);
        }

        //public async Task<IActionResult> CreateDetails(Budget budget)
        //{
        //    if (budget.BudgetCategory.Count == 0)
        //    {
        //        return RedirectToAction("Edit", new { id = budget.BudgetId });
        //    }


        //    BudgetViewModel model = new BudgetViewModel();
        //    model.Budget = budget;
        //    model.BudgetCategories = await _context.BudgetCategory.Where(bc => bc.BudgetId == budget.BudgetId).ToListAsync();

        //    return View(model);
        //}



        public IActionResult CreateDetails(BudgetViewModel model)
        {
            if (model.BudgetCategories == null || model.BudgetCategories.Count == 0)
            {
                return RedirectToAction("Edit", new { id = model.Budget.BudgetId });
            }

            return View(model);
        }


        [ValidateAntiForgeryToken]
        [Authorize]
        [HttpPost, ActionName("CreateDetails")]
        public async Task<IActionResult> CreateDetailsConfirm([Bind("Name,BudgetPerDay,BudgetCategoryId,BudgetId")] List<BudgetCategory> categories)
        {
                foreach (var bc in categories)
                {

                    _context.Update(bc);
                }
                await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Budgets");
            
        }


        // GET: Budgets/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var budget = await _context.Budget.FindAsync(id);

           ViewBag.data = new string[] { "Transportation", "Lodging", "Food", "Activities", "Misc", "Non-Daily Expenses (Untracked)" };

            if (budget == null)
            {
                return NotFound();
            }
            return View(budget);
        }

        // POST: Budgets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string[] categories, [Bind("BudgetId, UserId, Name,StartDate,EndDate,BudgetAmount,IsActive")] Budget budget)
        {
            if (id != budget.BudgetId)
            {
                return NotFound();
            }
            
            ModelState.Remove("User");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(budget);
                    await _context.SaveChangesAsync();

                    if (budget.IsActive)
                    {
                        var makeInactive = await _context.Budget.Where(b => b.IsActive && b.BudgetId != budget.BudgetId).FirstOrDefaultAsync();
                        if (makeInactive != null)
                        {
                            makeInactive.IsActive = false;
                            _context.Update(makeInactive);
                        }

                    }



                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BudgetExists(budget.BudgetId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                var budgetCategories = await _context.BudgetCategory.Where(bc => bc.BudgetId == budget.BudgetId).ToListAsync();

                foreach (string category in categories)
                {
                    if (!budgetCategories.Exists(bc => bc.Name == category))
                    {
                        BudgetCategory bc = new BudgetCategory()
                        {
                            Name = category,
                            BudgetId = budget.BudgetId,
                            BudgetPerDay = 0,
                        };
                        _context.BudgetCategory.Add(bc);
                    }
                }
                    await _context.SaveChangesAsync();

                BudgetViewModel model = new BudgetViewModel
                {
                    Budget = budget,
                    BudgetCategories = await _context.BudgetCategory.Where(bc => bc.BudgetId == budget.BudgetId).ToListAsync()
                };

                return View("CreateDetails", model);

            }

            return View(budget);
        }

        //GET: Budgets/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var budget = await _context.Budget
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.BudgetId == id);
            if (budget == null)
            {
                return NotFound();
            }

            return View(budget);
        }

        // POST: Budgets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var budgetToDelete = await _context.Budget.FindAsync(id);
            _context.Budget.Remove(budgetToDelete);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BudgetExists(int id)
        {
            return _context.Budget.Any(e => e.BudgetId == id);
        }
    }
}
