using CustomerPortal.Models;
using CustomerPortal.Services;
using CustomerPortal.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace CustomerPortal.Controllers;

public class CustomerController: Controller
{
    private readonly ICustomerService _customerService;
    
    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }
    
    public async Task<IActionResult> Index() {
        var loggedCustomerId = HttpContext.Session.GetInt32("CustomerID");

        if (loggedCustomerId == null)
        {
            return RedirectToAction("Index", "Home");
        }
        
        var customer = await _customerService.GetCustomer(loggedCustomerId.Value);

        var model = new CustomerViewModel()
        {
            CustomerID = customer.CustomerID,
            Name = customer.Name,
            TFN = customer.TFN,
            Address = customer.Address,
            City = customer.City,
            State = customer.State,
            PostCode = customer.PostCode,
            Mobile = customer.Mobile
        };

        return View(model);
    }
    
    [HttpPost]
    public async Task<IActionResult> UpdateCustomer(Customer customerData)
    {
        var loggedCustomerId = HttpContext.Session.GetInt32("CustomerID");
        var model = new CustomerViewModel();

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
        model.CustomerID = updateResult.CustomerID;
        model.Name = updateResult.Name;
        model.TFN = updateResult.TFN;
        model.Address = updateResult.Address;
        model.City = updateResult.City;
        model.State = updateResult.State;
        model.PostCode = updateResult.PostCode;
        model.Mobile = updateResult.Mobile;
        model.Message = "Customer Data update Success";
        
        return View("Index", model);

        //return RedirectToAction("Index"); 
    }
}