using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace BackpackingBudget.Models
{
    public class Budget
    {
        [Key]
        public int BudgetId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime? EndDate { get; set; }
        
        [Required]
        [Display(Name = "Budget Amount")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal BudgetAmount { get; set; }
        [Required]
        public virtual ApplicationUser User { get; set; }
        [Required]
        [Display(Name = "Active Budget")]
        public bool IsActive { get; set; }
        public virtual ICollection<BudgetCategory> BudgetCategory { get; set; }


    }
}
