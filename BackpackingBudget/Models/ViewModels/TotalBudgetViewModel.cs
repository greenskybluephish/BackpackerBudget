using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BackpackingBudget.Models.ViewModels
{
    public class TotalBudgetViewModel
    {
        public Budget Budget { get; set; }

        public List<CategoryViewModel> CategoryViewModels { get; set; } = new List<CategoryViewModel>();

        public decimal AmountSpent { get; set; }


        public decimal AmountRemaining()
        {
            return Budget.BudgetAmount - AmountSpent;
        }
        [DataType(DataType.Currency)]
        public decimal EstimatedPerDayAverage()
        {
            return Budget.BudgetAmount / TotalDays();
        }
        [DataType(DataType.Currency)]
        public double ActualPerDayAverage()
        {
            return (double)AmountSpent / DaysSinceStart();
        }

        public double DaysLeftAtCurrentSpendingRate()
        {
            return ActualPerDayAverage() / (TotalDays() - DaysSinceStart());
        }

        public int DaysLeft() {
            return (int)Math.Floor(DaysLeftAtCurrentSpendingRate());
                }

        [DataType(DataType.Date)]
        public DateTime EstimatedReturnDate()
        {
           return DateTime.Now.AddDays(DaysLeftAtCurrentSpendingRate()).Date;
        }


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
