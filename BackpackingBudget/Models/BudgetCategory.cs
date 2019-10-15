using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BackpackingBudget.Models

{
    public class BudgetCategory
    {

        [Key]
        [Display(Name = "Category")]
        public int BudgetCategoryId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Estimated Daily Expense")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:C}")]
        [DataType(DataType.Currency)]
        public decimal BudgetPerDay { get; set; }

        [Required]
        public int BudgetId { get; set; }
        public virtual Budget Budget { get; set; }
        public virtual ICollection<BudgetItem> BudgetItem { get; set; }
    }
}
