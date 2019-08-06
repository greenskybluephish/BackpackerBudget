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
        public int BudgetCategoryId { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Cost { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}")]
        public DateTime PurchaseDate { get; set; }
        public string Description { get; set; }

        public virtual BudgetCategory BudgetCategory { get; set; }
    }
}
