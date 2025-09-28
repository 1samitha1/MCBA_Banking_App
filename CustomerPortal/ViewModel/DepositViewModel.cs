using System.ComponentModel.DataAnnotations;
using CustomerPortal.Models;
using CustomerPortal.Utility;

namespace CustomerPortal.ViewModel;

public class DepositViewModel
{
    [Required]
    public int AccountNumber { get; set; }
    public List<Account> Accounts { get; set; } = new List<Account>();
    [Required]
    [DataType(DataType.Currency)]
    public decimal Amount { get; set; }
    
    [Display(Name = "Description (optional")]
    [StringLength(200)]
    public string? Comment { get; set; }
    public bool IsOk { get; set; }

    

}