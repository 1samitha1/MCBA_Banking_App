using CustomerPortal.Models;
using CustomerPortal.Services;
using CustomerPortal.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace CustomerPortal.Controllers;

public class CustomerController: Controller
{
    private readonly ICustomerService _customerService;
    private readonly IPasswordService _passwordService;
    
    public CustomerController(ICustomerService customerService, IPasswordService passwordService)
    {
        _customerService = customerService;
        _passwordService = passwordService;
    }
    
    public async Task<IActionResult> Index() {
        var loggedCustomerId = HttpContext.Session.GetInt32("CustomerID");

        if (loggedCustomerId == null)
        {
            return RedirectToAction("Index", "Home");
        }
        
        var customer = await _customerService.GetCustomer(loggedCustomerId.Value);

        var model = new CustomerViewModel(customer);
        return View(model);
    }
    
    [HttpPost]
    public async Task<IActionResult> UpdateCustomer(Customer customerData)
    {
        var loggedCustomerId = HttpContext.Session.GetInt32("CustomerID");
        var customer = await _customerService.GetCustomer(loggedCustomerId.Value);
        var model = new CustomerViewModel(customer);

        // update customer data
        var updateResult = await _customerService.UpdateCustomer(loggedCustomerId.Value, customerData);

        // If update failed, show an error message
        if (updateResult == null)
        {
            model.Message = "Customer Data update Failed";
            model.IsSuccess = false;
        }

        // bind new data with view model
        model.IsSuccess = true;
        model.Message = "Customer Data update Success";
        
        return View("Index", model);
        
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword, string confirmPassword)
    {
        var loggedCustomerId = HttpContext.Session.GetInt32("CustomerID");
        var customer = await _customerService.GetCustomer(loggedCustomerId.Value);

        // verify password and password confirmation
        if (newPassword != confirmPassword)
        {
            var model = new CustomerViewModel(customer)
            {
                PasswordChangeMessage = "New password and password confirmation do not match",
                IsPasswordChangeSuccess = false
            };
            return View("Index", model);
        }
        
        // change password
        var (isSuccess, message) = await _passwordService.ChangePassword(loggedCustomerId.Value, currentPassword, newPassword);
        
        // update view
        var viewModel = new CustomerViewModel(customer)
        {
            PasswordChangeMessage = message,
            IsPasswordChangeSuccess = isSuccess,
            CurrentPassword = string.Empty,
            NewPassword = string.Empty,
            ConfirmPassword = string.Empty
        };

        return View("Index", viewModel);
    }
    
}