using System.ComponentModel.DataAnnotations;

namespace AdminPortal.ViewModels;

public class LoginViewModel
{
    [Required]
    [Display(Name = "Login ID")]
    public string LoginID { get; set; } = default!;

    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = default!;
}