using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CustomerPortal.Models;

public class Transaction
{
    [Key]
    public int TransactionID { get; set; }
    
    [Required]
    public TransactionType TransactionType { get; set; }
    [Required]
    public int AccountNumber { get; set; }
    
    public int? DestinationAccountNumber { get; set; }
    [Required, Column(TypeName = "money")]
    public decimal Amount { get; set; }
    [StringLength(30)]
    public string? Comments { get; set; }
    [Required]
    public DateTime TransactionTimeUtc { get; set; }
    
    public Account Account { get; set; } = null!;
    public Account DestinationAccount { get; set; }
}