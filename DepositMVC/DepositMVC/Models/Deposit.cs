
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DepositMVC.Models
{
    public class Deposit
    {
        
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal ToPrice { get; set; }
        public decimal FromPrice { get; set; }
        [Required]
        public int Days { get; set; }
        [Required]
        public decimal Accrual { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public IEnumerable<UserDeposit> UserDeposits { get; set; }
    }
}
