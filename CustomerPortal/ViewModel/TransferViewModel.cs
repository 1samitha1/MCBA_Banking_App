using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CustomerPortal.Models;

namespace CustomerPortal.ViewModel;

public class TransferViewModel
{
    [Required]
    [Display(Name = "Source Account")]
    public int SourceAccountNumber { get; set; } 
    
    [Required]
    [Display(Name = "Destination Account")]
    public int DestinationAccountNumber { get; set; }

    [Required]
    [Range(1, double.MaxValue, ErrorMessage = "Amount must be greater than zero")]
    public decimal Amount { get; set; }
    
    public List<Account> CustomerAccounts { get; set; } = new List<Account>();
    public List<Account> DestinationAccounts { get; set; } = new List<Account>();
    
    [DefaultValue(null)]
    public string? Comment { get; set; }
    
    public string? Message { get; set; }
    public bool IsSuccess { get; set; }

}