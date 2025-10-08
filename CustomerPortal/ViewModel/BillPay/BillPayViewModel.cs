using CustomerPortal.Utility;

namespace CustomerPortal.ViewModel.BillPay;

public class BillPayViewModel
{
    public int BillPayID { get; set; }

    public int AccountNumber { get; set; }
    
    public string PayeeID { get; set; }
   
    public decimal Amount { get; set; }

    public BillPeriod BillPeriod { get; set; }

    public DateTime ScheduleTimeUtc { get; set; }

    public string LocalScheduleTime => ScheduleTimeUtc.ToString("yyyy-MM-dd hh:mm tt"); 

    public BillStatus Status { get; set; }
    
    public string? ErrorMessage { get; set; }
}