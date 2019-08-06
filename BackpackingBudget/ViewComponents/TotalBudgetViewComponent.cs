
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using BackpackingBudget.Data;
//using BackpackingBudget.Models;
//using Microsoft.AspNetCore.Identity;
//using BackpackingBudget.Models.ViewModels;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.Http;
//using System.Linq;
//using System;

//namespace BackpackingBudget.Views.Shared.ViewComponents
//{
//    public class TotalBudgetViewComponent : ViewComponent
//    {
//        private readonly ApplicationDbContext _context;

//        private readonly UserManager<ApplicationUser> _userManager;
//        public TotalBudgetViewComponent(ApplicationDbContext c, UserManager<ApplicationUser> userManager)
//        {
//            _context = c;
//            _userManager = userManager;
//        }

//        public async Task<IViewComponentResult> InvokeAsync()
//        {
//            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

//            TotalBudgetViewModel TotalModel = new TotalBudgetViewModel();

//            var budget = await _context.Budget.Include(b => b.BudgetCategory)
//                .ThenInclude(bc => bc.BudgetItem)
//                .Where(b => b.User == currentUser && b.IsActive).FirstOrDefaultAsync();
//            var budgetItems = await _context.BudgetItem.Include(b => b.BudgetCategory).ThenInclude(bc => bc.Budget).Where(bi => bi.BudgetCategory.Budget == budget).ToListAsync();
            
//            var cost = budgetItems.Select(c => c.Cost).Sum();
         

//            TotalModel.Budget = budget;
//            TotalModel.AmountSpent = cost;
//            CategoryViewModel CategoryModel = new CategoryViewModel();
//            foreach (BudgetCategory bc in budget.BudgetCategory)
//            {
//                var categoryCost = await _context.BudgetItem.Where(bi => bi.BudgetCategoryId == bc.BudgetCategoryId).Select(bi => bi.Cost).ToListAsync();
//                CategoryModel = new CategoryViewModel()
//                {
//                    BudgetCategory = bc,
//                    TotalDays = TotalModel.TotalDays(),
//                    DaysRemaining = TotalModel.TotalDays() - TotalModel.DaysSinceStart(),
//                    TotalSpent = categoryCost.Sum()
//                };
//                TotalModel.CategoryViewModels.Add(CategoryModel); 
//            }

//            return View(TotalModel);
//        }
//    }
//}
