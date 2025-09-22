using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerPortal.Models;

[Table("Payee")]
public class Payee
{
    [Key]
    public int PayeeID { get; set; }
    [Required, StringLength(50)]
    public required string Name { get; set; } = string.Empty;
    [Required, StringLength(40)]
    public required string Address { get; set; } = string.Empty;
    [Required, StringLength(40)]
    public required string City { get; set; } = string.Empty;
    [Required, StringLength(3)]
    public required string State { get; set; }  = string.Empty;
    [StringLength(4)]
    public string? PostCode { get; set; }
    [Required, StringLength(14)]
    public required string Phone { get; set; } = string.Empty;
    
    //nav
    public ICollection<BillPay> BillPays { get; set; } = new List<BillPay>();
}