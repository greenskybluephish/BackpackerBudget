using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackpackingBudget.Models.ViewModels
{
    public class CategoryViewModel
    {
        public BudgetCategory BudgetCategory { get; set; }

        public decimal TotalSpent { get; set; }


        public int TotalDays { get; set; }

        public int DaysRemaining { get; set; }

        public decimal TotalEstimated()
        {
            return BudgetCategory.BudgetPerDay * TotalDays;
        }

        public decimal AmountRemaining()
        {
            return (TotalEstimated() - TotalSpent );
        }

        public decimal AveragePerDay()
        {
            return TotalSpent / (TotalDays - DaysRemaining);
        }
        
        public decimal AveragePerDayRemaining()
        {
            return AmountRemaining() / DaysRemaining;
        }
    }
}
