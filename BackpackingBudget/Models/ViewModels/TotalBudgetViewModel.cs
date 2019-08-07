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

        public List<ChartDataViewModel> ChartDataModels { get; set; } = new List<ChartDataViewModel>();

        public List<ChartDataViewModel> ChartDataModels1 { get; set; } = new List<ChartDataViewModel>();
        public List<ChartDataViewModel> ChartDataModels2 { get; set; } = new List<ChartDataViewModel>();
        public List<ChartDataViewModel> ChartDataModels3 { get; set; } = new List<ChartDataViewModel>();

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
            if (DaysSinceStart() == 0)
            {
                return (double)EstimatedPerDayAverage();
            }

            return (double)AmountSpent / DaysSinceStart();
        }

        public double DaysLeftAtCurrentSpendingRate()
        {
            if (ActualPerDayAverage() == 0)
            {
                return (double)TotalDays();
            }

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
            if (today.Day == Budget.StartDate.Day)
            {
                return 0;
            }
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
