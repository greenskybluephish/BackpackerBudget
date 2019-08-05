using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackpackingBudget.Models.ViewModels
{
    public class CategoryViewModel
    {
        public BudgetCategory BudgetCategory { get; set; }

        public decimal TotalSpent()
        {
            return BudgetCategory.BudgetItem.Select(bi => bi.Cost).ToList().Sum();
        }

        public int Days { get; set; }

        public decimal TotalEstimated()
        {
            return BudgetCategory.BudgetPerDay * Days;
        }

        public decimal AmountRemaining()
        {
            return (TotalSpent() - TotalEstimated());
        }

        public decimal AveragePerDay()
        {
            return TotalSpent() / Days;
        }
        
        public decimal AveragePerDayRemaining()
        {
            return AmountRemaining() / Days;
        }
    }
}
