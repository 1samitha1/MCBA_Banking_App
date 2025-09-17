using System.ComponentModel.DataAnnotations;

namespace CustomerPortal.Models;

public class Login
{
    [Key, StringLength(8)]
    public string LoginId { get; set; }

    [Required]
    public string CustomerId { get; set; }

    [Required, StringLength(94)]
    public string PasswordHash { get; set; } = string.Empty;
    
    public Customer customer{get;set;}
}