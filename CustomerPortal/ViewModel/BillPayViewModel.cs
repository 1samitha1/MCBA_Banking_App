using CustomerPortal.Utility;

namespace CustomerPortal.ViewModel;

public class BillPayViewModel
{
    public int BillPayID { get; set; }

    public int AccountNumber { get; set; }

    public string AccountName { get; set; } = string.Empty; 

    public int PayeeID { get; set; }
   
    public decimal Amount { get; set; }

    public BillPeriod BillPeriod { get; set; }

    public DateTime ScheduleTimeUtc { get; set; }

    public string LocalScheduleTime => ScheduleTimeUtc.ToLocalTime().ToString("g"); 
    // formatted local time for display

    public string Status { get; set; } = string.Empty; // Pending, Completed, Failed, etc.

    // For error handling if payment fails
    public string? Message { get; set; }
    
    public bool IsSuccess { get; set; }
}