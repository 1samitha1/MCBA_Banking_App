using CustomerPortal.Data.Repository;
using CustomerPortal.Services;
using CustomerPortal.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerPortal.Controllers;


public class DepositController :Controller
{
    private readonly IAccountRepository _accountRepository;
    private readonly IAuthService _authService;
    private readonly IDepositService _depositService;

    public DepositController(IAccountRepository accountRepository, IAuthService authService, IDepositService depositService)
    {
        _accountRepository = accountRepository;
        _authService = authService;
        _depositService = depositService;
    }

    public IActionResult Index()
    {
        var currentCustomerId = _authService.CurrentCustomerId();
        if (currentCustomerId is null)
        {
            return View("Index", "Login");
        }
        int signedCustomerId = currentCustomerId.Value;
        var customerAccounts = _accountRepository.GetCustomerAccounts(signedCustomerId);
        var model = new DepositViewModel();
        model.Accounts = customerAccounts.Result;
        Console.WriteLine(model.AccountNumber);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> HandleDeposit(DepositViewModel model)
    {
        var customerId = _authService.CurrentCustomerId();
        if (customerId is null)
            return RedirectToAction("Index", "Login");

        if (!ModelState.IsValid)
            return View("Index", model);
        try
        {
            await _depositService.DepositAsync(model, customerId.Value);
            model.IsOk = true;
            return RedirectToAction("DepositSuccess", new
            {
                accountNumber = model.AccountNumber,
                amount = model.Amount
            } );
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            model.IsOk = false;
            ModelState.AddModelError(string.Empty, "Deposit failed. Try again!");
            return View("Index", model);
        }
        
    }
    
    [HttpGet]
    public IActionResult DepositSuccess(int accountNumber, decimal amount)
    {
        var viewModel = new DepositViewModel
        {
            AccountNumber = accountNumber,
            Amount = amount
        };
        return View(viewModel);
    }
}