using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerPortal.Models;

[Table("Login")]
public class Login
{
    [Key, StringLength(8)] 
    public string LoginID { get; set; } = string.Empty;

    [Required] 
    public int CustomerID { get; set; }

    [Required, StringLength(94)]
    public string PasswordHash { get; set; } = string.Empty;

    public Customer Customer { get; set; } = null!;
}