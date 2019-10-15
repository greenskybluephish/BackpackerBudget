using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BackpackingBudget.Models
{
    public class BudgetItem
    {
        [Key]
        public int BudgetItemId { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int BudgetCategoryId { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:C}")]
        [DataType(DataType.Currency)]
        public decimal Cost { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime PurchaseDate { get; set; }
        public string Description { get; set; }

        public virtual BudgetCategory BudgetCategory { get; set; }
    }
}
