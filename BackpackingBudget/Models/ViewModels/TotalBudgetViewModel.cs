using Microsoft.AspNetCore.Mvc.Rendering;
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

        public BudgetItem BudgetItem { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}")]
        [DataType(DataType.Currency)]
        public decimal AmountSpent { get; set; }


        public double  AmountRemaining()
        {
            return (double) (Budget.BudgetAmount - AmountSpent);
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
            return AmountRemaining() / ActualPerDayAverage();
        }

        public int DaysLeft() {
            return (int)Math.Floor(DaysLeftAtCurrentSpendingRate());
                }

        public string EstimatedReturnDate()
        {
           return DateTime.Now.AddDays(DaysLeftAtCurrentSpendingRate()).Date.ToLongDateString();
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

        public string DateToString(DateTime d)
        {
            return d.ToString("d");
        }

        public string DoubleToCurrrency(Double d)
        {
            return d.ToString("C");
        }
    }
}
