using CustomerPortal.Services;
using CustomerPortal.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace CustomerPortal.Controllers;

public class BillPayController: Controller
{
    private readonly IBillPayService _billPayService;
    
    public BillPayController(IBillPayService billPayService)
    {
        _billPayService =  billPayService;
    }

    public async Task<IActionResult> Index()
    {
        var loggedCustomerId = HttpContext.Session.GetInt32("CustomerID");
        
        if (loggedCustomerId == null)
        {
            return RedirectToAction("Index", "Home");
        }

        var bills = await _billPayService.GetBills(loggedCustomerId.Value);

        var vm = new BillPayListViewModel
        {
            Bills = bills.Select(b => new BillPayViewModel
            {
                BillPayID = b.BillPayID,
                AccountNumber = b.AccountNumber,
                PayeeID = b.PayeeID,
                Amount = b.Amount,
                BillPeriod = b.BillPeriod,
                ScheduleTimeUtc = b.ScheduleTimeUtc,
                Status = "Pending"
            }).ToList()
        };

        return View(vm);
    }
}