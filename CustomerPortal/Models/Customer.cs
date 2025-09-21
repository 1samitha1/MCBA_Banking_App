using System.ComponentModel.DataAnnotations;

namespace CustomerPortal.Models;

public class Customer
{
    [Key]
    public int CustomerID { get; set; }                  // PK (not identity in SQL)

    [Required, StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(11)]
    public string TFN { get; set; } = string.Empty;

    [StringLength(50)]
    public string? Address { get; set; }

    [StringLength(40)]
    public string? City { get; set; }

    [StringLength(3)]
    public string? State { get; set; }

    [StringLength(4)]
    public string? PostCode { get; set; }

    [StringLength(12)]
    public string? Mobile { get; set; }

    // navs
    public ICollection<Login> Logins { get; set; } = new List<Login>();
    
    public ICollection<Account> Accounts { get; set; } = new List<Account>();
}