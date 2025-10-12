using System.ComponentModel.DataAnnotations;
using CustomerPortal.Utility;

namespace AdminPortal.ViewModels;

public class BillPayViewModel
{
    [Required]
    public int BillPayID { get; set; }
    [Required]
    public int AccountNumber { get; set; }
    [Required]
    public string PayeeName { get; set; } = "";
    [Required]
    public decimal Amount { get; set; }
    public DateTime ScheduleTimeLocal { get; set; }
    [Required]
    public BillPeriod Period { get; set; }    
    public bool IsBlocked { get; set; }
}