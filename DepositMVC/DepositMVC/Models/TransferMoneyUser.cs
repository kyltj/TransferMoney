using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DepositMVC.Models
{
    public class TransferMoneyUser
    {

        public int Id { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        [Required]
        public bool SetGet { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}