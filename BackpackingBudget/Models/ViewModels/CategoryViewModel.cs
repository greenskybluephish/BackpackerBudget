using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BackpackingBudget.Models.ViewModels
{
    public class CategoryViewModel
    {
        public BudgetCategory BudgetCategory { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}")]
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

        public string AveragePerDay()
        {
            return (TotalSpent / (TotalDays - DaysRemaining)).ToString("C");
        }
        
        public string AveragePerDayRemaining()
        {
            return (AmountRemaining() / DaysRemaining).ToString("C");
        }

        public string DecimalToCurrrency(Decimal d)
        {
            return d.ToString("C");
        }
    }
}
