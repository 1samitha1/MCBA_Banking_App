using System.ComponentModel.DataAnnotations;

namespace CustomerPortal.Models;

public class Customer
{
    [Key]
    [Range(1000, 9999, ErrorMessage = "CustomerID must be 4 digits.")]
    public int CustomerID { get; set; }

    [Required, StringLength(50)]
    public string Name { get; set; } = string.Empty;

    // Optional in spec except format; make nullable with regex validation when provided
    [StringLength(11)]
    [RegularExpression(@"^\d{3}\s\d{3}\s\d{3}$", ErrorMessage = "TFN must be XXX XXX XXX.")]
    public string? TFN { get; set; }

    [StringLength(50)]
    public string? Address { get; set; }

    [StringLength(40)]
    public string? City { get; set; }

    [StringLength(3)]
    [RegularExpression(@"^[A-Z]{2,3}$", ErrorMessage = "State must be a 2 or 3 letter Australian state code.")]
    public string? State { get; set; }

    [StringLength(4)]
    [RegularExpression(@"^\d{4}$", ErrorMessage = "Postcode must be 4 digits.")]
    public string? Postcode { get; set; }

    [StringLength(12)]
    [RegularExpression(@"^04\d{2}\s\d{3}\s\d{3}$", ErrorMessage = "Mobile must be 04XX XXX XXX.")]
    public string? Mobile { get; set; }

    // Navigation
    public ICollection<Account> Accounts { get; set; } = new List<Account>();
    public Login Login { get; set; } = null!;
}