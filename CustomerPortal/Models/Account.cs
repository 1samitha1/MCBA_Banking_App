using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerPortal.Models;

[Table("Account")]
public class Account
{
    [Key]
    public int AccountNumber { get; set; }               // PK (not identity)

    [Required]
    public AccountType AccountType { get; set; }         // char(1) via converter

    [Required]
    public int CustomerID { get; set; }

    [Required,Column(TypeName = "money")]
    public decimal Balance { get; set; }

    // navs
    public Customer Customer { get; set; } = null!;
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public ICollection<BillPay> BillPays { get; set; } = new List<BillPay>();
}