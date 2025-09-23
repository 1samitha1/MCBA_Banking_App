using System.ComponentModel.DataAnnotations;

namespace CustomerPortal.ViewModel;

public class LoginViewModel
{
    [Required, StringLength(8, MinimumLength = 8)]
    [Display(Name = "Login ID")]
    public string LoginID { get; set; } = default!;

    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = default!;
}