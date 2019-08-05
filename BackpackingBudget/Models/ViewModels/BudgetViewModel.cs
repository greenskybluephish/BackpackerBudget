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
    }
}
