using System.Diagnostics;
using CustomerPortal.Data;
using Microsoft.AspNetCore.Mvc;
using CustomerPortal.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerPortal.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly McbaContext mcbacontext;

    public HomeController(ILogger<HomeController> logger, McbaContext context)
    {
        _logger = logger;
        mcbacontext = mcbacontext;
    }

    public async Task<IActionResult> Index()
    {
      //  return View();
      
      // var accounts =  await mcbacontext.Accounts
      //     .Where(a => a.CustomerID == 2100)
      //     .ToListAsync();
      
      var accounts = new List<Account>
      {
          new Account { AccountNumber = 12345678, AccountType = AccountType.S, Balance = 2500.75m },
          new Account { AccountNumber = 87654321, AccountType = AccountType.S, Balance = 1250.00m }
      };

      return View(accounts);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
