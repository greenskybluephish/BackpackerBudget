﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? EndDate { get; set; }

        [Required]
        [Display(Name = "Budget Amount")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        [DataType(DataType.Currency)]
        public decimal BudgetAmount { get; set; }

        [Required]
        public virtual ApplicationUser User { get; set; }

        [Required]
        [Display(Name = "Active Budget")]
        public bool IsActive { get; set; }
        public virtual List<BudgetCategory> BudgetCategory { get; set; }

        public DateTime EndDateExists()
        {
            return (DateTime)EndDate;
        }

    }
}
