
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

        public async Task<IActionResult> Dashboard()
        {
            var currentUser = await GetCurrentUserAsync();

            TotalBudgetViewModel TotalModel = new TotalBudgetViewModel();

            var budget = await _context.Budget.Include(b => b.BudgetCategory)
                .ThenInclude(bc => bc.BudgetItem)
                .Where(b => b.User == currentUser && b.IsActive).FirstOrDefaultAsync();
            var budgetItems = await _context.BudgetItem.Include(b => b.BudgetCategory).ThenInclude(bc => bc.Budget).Where(bi => bi.BudgetCategory.Budget == budget).ToListAsync();

            var cost = budgetItems.Select(c => c.Cost).Sum();

            TotalModel.Budget = budget;
            TotalModel.AmountSpent = cost;


            //int TotalDays()
            //{
            //    var endDate = (DateTime)budget.EndDate;

            //    return endDate.Subtract(budget.StartDate).Days;
            //}

            //int DaysSinceStart()
            //{
            //    DateTime today = DateTime.Now;
            //    return today.Subtract(budget.StartDate).Days;
            //}

            foreach (BudgetCategory bc in budget.BudgetCategory)
            {
                var categoryCost = await _context.BudgetItem.Where(bi => bi.BudgetCategoryId == bc.BudgetCategoryId).Select(bi => bi.Cost).ToListAsync();
                CategoryViewModel CategoryModel = new CategoryViewModel()
                {
                    BudgetCategory = bc,
                    TotalDays = TotalModel.TotalDays(),
                    DaysRemaining = TotalModel.TotalDays() - TotalModel.DaysSinceStart(),
                    TotalSpent = categoryCost.Sum()
                };
                TotalModel.CategoryViewModels.Add(CategoryModel);
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
