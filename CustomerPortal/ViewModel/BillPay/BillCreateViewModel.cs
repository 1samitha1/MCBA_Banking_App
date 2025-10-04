using System.ComponentModel.DataAnnotations;
using CustomerPortal.Utility;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CustomerPortal.ViewModel.BillPay;

public class BillCreateViewModel
{
    [Required]
    public int AccountNumber { get; set; }

    [Required]
    public int PayeeID { get; set; }

    [Required, Range(0.01, double.MaxValue, ErrorMessage = "Amount must be positive")]
    public decimal Amount { get; set; }
    
    [Required,  DataType(DataType.DateTime)]
    public DateTime ScheduleTimeUtc { get; set; } = DateTime.UtcNow;

    [Required]
    public BillPeriod BillPeriod { get; set; }
    
    [Required]
    public BillStatus Status { get; set; }
    
    // For dropdown lists
    public List<SelectListItem> Accounts { get; set; } = new();
    public List<SelectListItem> Payees { get; set; } = new();
}