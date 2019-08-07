
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BackpackingBudget.Data;
using BackpackingBudget.Models;
using Microsoft.AspNetCore.Identity;
using BackpackingBudget.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BackpackingBudget.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;
        public HomeController(ApplicationDbContext c, UserManager<ApplicationUser> userManager)
        {
            _context = c;
            _userManager = userManager;
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
        [Authorize]        
        public async Task<IActionResult> Dashboard()
        {
            var currentUser = await GetCurrentUserAsync();

            TotalBudgetViewModel TotalModel = new TotalBudgetViewModel();

            var budget = await _context.Budget.Include(b => b.BudgetCategory)
                .ThenInclude(bc => bc.BudgetItem)
                .Where(b => b.User == currentUser && b.IsActive).FirstOrDefaultAsync();

            if (budget == null || budget.BudgetCategory.Count == 0)
            {
                return RedirectToAction("Index", "Budgets");
            }

            if (budget.StartDate > DateTime.Now || budget.EndDate < DateTime.Now)
            {
                return RedirectToAction("Index", "Budgets");
            }

            var budgetItems = await _context.BudgetItem.Include(b => b.BudgetCategory).ThenInclude(bc => bc.Budget).Where(bi => bi.BudgetCategory.Budget == budget).ToListAsync();

            var cost = budgetItems.Select(c => c.Cost).Sum();

            TotalModel.Budget = budget;
            TotalModel.AmountSpent = cost;
            ViewData["BudgetCategoryId"] = new SelectList(budget.BudgetCategory, "BudgetCategoryId", "Name");

            decimal costIfZero()
            {
                if (cost == 0)
                {
                    // perform the division only if cost is different than 0,
                    return 1;
                }
                else
                {
                    return cost;
                }

            }

            int zeroDay()
            {
                if (TotalModel.DaysSinceStart()==0)
                {
                    return 1;
                }
                else
                {
                    return TotalModel.DaysSinceStart();
                }
            }

            foreach (BudgetCategory bc in budget.BudgetCategory)
            {
                var categoryCost = await _context.BudgetItem.Where(bi => bi.BudgetCategoryId == bc.BudgetCategoryId).Select(bi => bi.Cost).ToListAsync();
                CategoryViewModel CategoryModel = new CategoryViewModel()
                {
                    BudgetCategory = bc,
                    TotalDays = TotalModel.TotalDays(),
                    DaysRemaining = TotalModel.TotalDays() - TotalModel.DaysSinceStart(),
                    TotalSpent = categoryCost.Sum(),
                };
                TotalModel.CategoryViewModels.Add(CategoryModel);
                ChartDataViewModel ChartData = new ChartDataViewModel()
                {
                    xValue = bc.Name,
                    yValue = (double)(categoryCost.Sum() / costIfZero()),
                    text = (categoryCost.Sum() / costIfZero()).ToString("P")
                };
                TotalModel.ChartDataModels.Add(ChartData);
                ChartDataViewModel ChartData1 = new ChartDataViewModel()
                {
                    xValue = bc.Name,
                    PerDay = (double)bc.BudgetPerDay
                };
                TotalModel.ChartDataModels1.Add(ChartData1);
                ChartDataViewModel ChartData2 = new ChartDataViewModel()
                {
                    xValue = bc.Name,
                    PerDay = (double)categoryCost.Sum() / zeroDay()
                };
                TotalModel.ChartDataModels2.Add(ChartData2);

            }

            return View(TotalModel);
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}
