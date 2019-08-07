using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackpackingBudget.Models.ViewModels
{
    public class BudgetViewModel
    {
        public Budget Budget { get; set; }

        public List<BudgetCategory> BudgetCategories { get; set; }

        public BudgetCategory BudgetCategory { get; set; }

        public int BudgetPerDay()
        {
            var bpd = Budget.BudgetAmount/ (Budget.EndDateExists().Subtract(Budget.StartDate).Days);
            return (int)Math.Floor(bpd);
        }
    }
}
