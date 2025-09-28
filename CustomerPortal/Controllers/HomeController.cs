using System.Diagnostics;
using CustomerPortal.Data.Repository;
using Microsoft.AspNetCore.Mvc;
using CustomerPortal.Models;
using CustomerPortal.ViewModel;

namespace CustomerPortal.Controllers;


public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IAccountRepository _accountRepository;

    public HomeController(ILogger<HomeController> logger, IAccountRepository accRepository)
    {
        _logger = logger;
        _accountRepository = accRepository;
    }
    public async Task<IActionResult> Index()
    {
        var loggedCustomerId = HttpContext.Session.GetInt32("CustomerID");

        // no customer logged in
        if (loggedCustomerId == null) {
            return RedirectToAction("Index", "Home");
        }

        var accountsList = await _accountRepository.GetCustomerAccounts(loggedCustomerId.Value);
        
        var model = accountsList.Select(a => new HomeViewModel
        {
            AccountNumber = a.AccountNumber,
            AccountType = a.AccountType,
            Balance = a.Balance
        }).ToList();

        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
