using CustomerPortal.Utility;

namespace CustomerPortal.ViewModel;
using System.ComponentModel.DataAnnotations;

public class HomeViewModel
{
    [Required]
    public int AccountNumber { get; set; } = default!;

    [Required]
    public AccountType AccountType { get; set; } = default!;

    [Required]
    public decimal Balance { get; set; } = default!;
}