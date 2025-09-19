using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerPortal.Models;

public class BillPay
{
    [Key]
    public int BillPayId { get; set; }
    [Required]
    public int AccountNumber { get; set; }
    [Required]
    public int PayeeId { get; set; }
    [Required, Column(TypeName = "Money")]
    public decimal Amount { get; set; }
    [Required]
    public DateTime ScheduleTimeUtc { get; set; }
    [Required]
    public BillPeriod BillPeriod { get; set; }
    
    public Account Account { get; set; }
    public Payee Payee { get; set; }
}