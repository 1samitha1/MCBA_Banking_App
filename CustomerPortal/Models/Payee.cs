using System.ComponentModel.DataAnnotations;

namespace CustomerPortal.Models;

public class Payee
{
    [Key]
    public int PayeeID { get; set; }
    [Required, StringLength(50)]
    public string Name { get; set; }
    [Required, StringLength(40)]
    public string Address { get; set; }
    [Required, StringLength(40)]
    public string City { get; set; }
    [Required, StringLength(3)]
    public string State { get; set; }
    [StringLength(4)]
    public string? PostCode { get; set; }
    [Required, StringLength(14)]
    public string Phone { get; set; } = string.Empty;
    
    public ICollection<BillPay> BillPays { get; set; } = new List<BillPay>();
}