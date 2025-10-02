using System.ComponentModel.DataAnnotations;
using CustomerPortal.Models;

namespace CustomerPortal.ViewModel;

public class CustomerViewModel
{
    public CustomerViewModel(Customer customer)
    {
        CustomerID = customer.CustomerID;
        Name = customer.Name;
        TFN = customer.TFN;
        Mobile = customer.Mobile;
        Address = customer.Address;
        City = customer.City;
        State = customer.State;
        PostCode = customer.PostCode;
    }

    [Required]
    public int CustomerID { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string TFN { get; set; }
    
    public string Address { get; set; }
    
    public string? City { get; set; }
    
    public string? State { get; set; }
    
    public string? PostCode { get; set; }
    
    public string? Mobile { get; set; }
    
    public string? Message { get; set; }
    public bool IsSuccess { get; set; }
    
    // password change
    [DataType(DataType.Password)]
    public string? CurrentPassword { get; set; }

    [DataType(DataType.Password)]
    public string? NewPassword { get; set; }

    [DataType(DataType.Password)]
    public string? ConfirmPassword { get; set; }
    public string? PasswordChangeMessage { get; set; } 
    
    public bool IsPasswordChangeSuccess { get; set; }
}