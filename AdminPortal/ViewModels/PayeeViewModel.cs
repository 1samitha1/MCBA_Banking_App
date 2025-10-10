using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AdminPortal.ViewModels;

public class PayeeViewModel
{ 
        public int PayeeID { get; set; }

        [Required(ErrorMessage = "Payee name is required")]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required")]
        [StringLength(40)]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "City is required")]
        [StringLength(40)]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "State is required")]
        [StringLength(3)]
        public string State { get; set; } = string.Empty;

        [StringLength(4)]
        public string? PostCode { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [StringLength(14)]
        [Phone(ErrorMessage = "Invalid phone number format")]
        public string Phone { get; set; } = string.Empty;
        
        // For selecting in a list
        public int selectedPostalCode { get; set; }
        public bool IsSelected { get; set; } = false;
        
}