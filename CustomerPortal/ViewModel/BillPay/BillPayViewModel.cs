using CustomerPortal.Utility;

namespace CustomerPortal.ViewModel.BillPay;

public class BillPayViewModel
{
    public int BillPayID { get; set; }

    public int AccountNumber { get; set; }
    
    public int PayeeID { get; set; }
   
    public decimal Amount { get; set; }

    public BillPeriod BillPeriod { get; set; }

    public DateTime ScheduleTimeUtc { get; set; }

    public string LocalScheduleTime => ScheduleTimeUtc.ToLocalTime().ToString("g"); 
    // formatted local time for display

    public BillStatus Status { get; set; }
}