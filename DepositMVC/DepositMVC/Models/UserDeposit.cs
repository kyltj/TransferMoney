using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DepositMVC.Models
{
    public class UserDeposit
    {

        [Key]
        public int Id { get; set; }

        [Required]

        public int DepositId { get; set; }
        public Deposit Deposit { get; set; }

        [Required]
        public int Days { get; set; }
        [Required]
        public decimal Price { get; set; }

        [Required]

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public DateTime DateEnd { get; set; }
        [Required]
        public decimal AccuralYet { get; set; }
    }
}
