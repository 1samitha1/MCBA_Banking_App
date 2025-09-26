using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CustomerPortal.Models;

namespace CustomerPortal.ViewModel;

public class WithdrawViewModel
{
    [Required]
    [Display(Name = "Account")]
    public int AccountNumber { get; set; } 

    [Required]
    [Range(1, double.MaxValue, ErrorMessage = "Amount must be greater than zero")]
    public decimal Amount { get; set; }
    
    public List<Account> Accounts { get; set; } = new List<Account>();
    
    [DefaultValue(null)]
    public string? Comment { get; set; }
    
    public string? Message { get; set; }
    public bool IsSuccess { get; set; }
}