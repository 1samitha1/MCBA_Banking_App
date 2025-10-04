using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CustomerPortal.Utility;

namespace CustomerPortal.Models;

[Table("BillPay")]
public class BillPay
{
    [Key]
    public int BillPayID { get; set; }
    [Required]
    public int AccountNumber { get; set; }
    [Required]
    public int PayeeID { get; set; }
    [Required, Column(TypeName = "Money")]
    public decimal Amount { get; set; }
    [Required]
    public DateTime ScheduleTimeUtc { get; set; }
    [Required]
    public BillPeriod BillPeriod { get; set; }
    
    [Required, DefaultValue(BillStatus.Pending)] 
    public BillStatus Status { get; set; }
    
    public Account Account { get; set; } = null!;
    public Payee Payee { get; set; } = null!;
}