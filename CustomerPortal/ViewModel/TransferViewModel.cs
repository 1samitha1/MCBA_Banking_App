using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CustomerPortal.Models;

namespace CustomerPortal.ViewModel;

public class TransferViewModel
{
    [Required]
    [Display(Name = "Account")]
    public int SourceAccNumber { get; set; } 
    
    [Required]
    [Display(Name = "Destination Account")]
    public int DestinationAccNumber { get; set; }

    [Required]
    [Range(1, double.MaxValue, ErrorMessage = "Amount must be greater than zero")]
    public decimal Amount { get; set; }
    
    public List<Account> CustomerAccounts { get; set; } = new List<Account>();
    public int DestinationAccount { get; set; }
    
    [DefaultValue(null)]
    public string? Comment { get; set; }
    
    public string? Message { get; set; }
    public bool IsSuccess { get; set; }

}