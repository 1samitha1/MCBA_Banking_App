using CustomerPortal.Data;
using CustomerPortal.Services;
using CustomerPortal.Services.Impl;
using CustomerPortal.Utility;
using CustomerPortal.ViewModel;
using Microsoft.AspNetCore.Mvc;
namespace CustomerPortal.Controllers;

//[RequireHttps]
public class LoginController : Controller
{
    private readonly IAuthService _authService;

    public LoginController(IAuthService authService)
    {
        _authService = authService;
    }

    public IActionResult Index()
    {
        return View(new LoginViewModel());
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> HandleLogin(LoginViewModel model)
    {
        //validate form input
        if (!ModelState.IsValid)
        {
            return View("Index", model);
        }
        
        //Authentication via auth service
        
        var (ok, message, customerId, customerName) = await _authService
            .SignInAsync(model.LoginID, model.Password);
        if (!ok)
        {
            ModelState.AddModelError(string.Empty, message ?? "Invalid Login ID or Password");
            // ModelState.Clear();
            return View("Index", model);
        }
        
        return RedirectToAction("Index","Home");
    }

    public IActionResult HandleLogout()
    {
        var model = new LoginViewModel();
        _authService.SignOut();
        return View("Index", model);
    }
    
}