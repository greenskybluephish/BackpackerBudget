
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

namespace BackpackingBudget.Views.Shared.ViewComponents
{
    public class TotalBudgetViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;
        public TotalBudgetViewComponent(ApplicationDbContext c, UserManager<ApplicationUser> userManager)
        {
            _context = c;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            TotalBudgetViewModel TotalModel = new TotalBudgetViewModel();

            var budget = await _context.Budget.Include(b => b.BudgetCategory)
                .ThenInclude(bc => bc.BudgetItem)
                .Where(b => b.User == currentUser && b.IsActive).FirstOrDefaultAsync();

            TotalModel.Budget = budget;

            foreach (BudgetCategory bc in budget.BudgetCategory)
            {
                CategoryViewModel CategoryModel = new CategoryViewModel()
                {
                    BudgetCategory = bc,
                    TotalEstimated = bc.BudgetPerDay * TotalModel.TotalDays(),
                    A
                };
                CategoryModel.AveragePerDay = CategoryModel.TotalSpent() / TotalModel.TotalDays();
            }

            return View(TotalModel);
        }
    }
}
