using System.ComponentModel.DataAnnotations;

namespace CustomerPortal.ViewModel;

public class CustomerViewModel
{
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
}