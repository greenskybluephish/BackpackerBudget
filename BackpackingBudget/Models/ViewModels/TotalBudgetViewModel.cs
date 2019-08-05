using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackpackingBudget.Models.ViewModels
{
    public class TotalBudgetViewModel
    {
        public Budget Budget { get; set; }

        public decimal AmountSpent { get; set; }

        public decimal EstimatedPerDayAverage { get; set; }

        public decimal ActualPerDayAverage { get; set; }

        public List<CategoryViewModel> CategoryViewModels { get; set; }

        public int TotalDays()
        {
            var endDate = (DateTime)Budget.EndDate;

            return endDate.Subtract(Budget.StartDate).Days;
        }

        public int DaysSinceStart()
        {
            DateTime today = DateTime.Now;
            return today.Subtract(Budget.StartDate).Days;
        }

    }
}
