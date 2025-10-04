using CustomerPortal.Models;
using CustomerPortal.Services;
using CustomerPortal.Utility;
using CustomerPortal.ViewModel;
using CustomerPortal.ViewModel.BillPay;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CustomerPortal.Controllers;

public class BillPayController: Controller
{
    private readonly IBillPayService _billPayService;
    private readonly IAccountService _accountService;
    private readonly IAuthService _authService;
    
    public BillPayController(IBillPayService billPayService, IAccountService accountService, IAuthService authService)
    {
        _billPayService =  billPayService;
        _accountService = accountService;
        _authService = authService;
    }

    public async Task<IActionResult> Index()
    {
        var loggedCustomerId = _authService.CurrentCustomerId();
        if (loggedCustomerId == null)
        {
            return RedirectToAction("Index", "Login");
        }

        var bills = await _billPayService.GetBills(loggedCustomerId.Value);

        var modal = new BillPayListViewModel
        {
            Bills = bills.Select(b => new BillPayViewModel
            {
                BillPayID = b.BillPayID,
                AccountNumber = b.AccountNumber,
                PayeeID = b.PayeeID,
                Amount = b.Amount,
                BillPeriod = b.BillPeriod,
                ScheduleTimeUtc = b.ScheduleTimeUtc,
                Status = b.Status
            }).ToList()
        };

        return View(modal);
    }

    public async Task<IActionResult> CreateView()
    {
        var loggedCustomerId = _authService.CurrentCustomerId();
        if (loggedCustomerId == null)
        {
            return RedirectToAction("Index", "Login");
        }
        
        var customerAccounts = await _accountService.GetCustomerAccounts(loggedCustomerId.Value);
        var payees = await _billPayService.GetPayees();
        
        var model = new BillCreateViewModel()
        {
            Accounts = customerAccounts.Select(a => new SelectListItem() 
            { 
                Value = a.AccountNumber.ToString(), 
                Text = $"{a.AccountNumber} - {a.AccountType}" 
            }).ToList(),
            
            Payees = payees.Select(p => new SelectListItem()
            { 
                Value = p.PayeeID.ToString(), 
                Text = $"{p.Name}" 
            }).ToList(),
        };
        
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(BillCreateViewModel vModel)
    {
        var loggedCustomerId = _authService.CurrentCustomerId();
        if (loggedCustomerId == null)
        {
            return RedirectToAction("Index", "Login");
        }
        
        var billPayObj = new BillPay
        {
            AccountNumber = vModel.AccountNumber,
            PayeeID = vModel.PayeeID,
            Amount = vModel.Amount,
            ScheduleTimeUtc = vModel.ScheduleTimeUtc, // <-- bound correctly now
            BillPeriod = vModel.BillPeriod, 
            Status = BillStatus.Pending
        };

        var (success, bills, msg) = await _billPayService.CreateBill(loggedCustomerId.Value, billPayObj);
        
        var billsViewModel = bills.Select(b => new BillPayViewModel
        {
            BillPayID = b.BillPayID,
            AccountNumber = b.AccountNumber,
            PayeeID = b.PayeeID,
            Amount = b.Amount,
            BillPeriod = b.BillPeriod,
            ScheduleTimeUtc = b.ScheduleTimeUtc,
            Status = b.Status, // assuming Status is enum
            
        }).ToList();

        var model = new BillPayListViewModel()
        {
            Message = msg,
            IsSuccess = success,
            Bills = billsViewModel
        };
        
        return View("Index", model);
    }

    [HttpGet]
    public async Task<IActionResult> Remove(int id)
    {
        var loggedCustomerId = _authService.CurrentCustomerId();
        var (success, msg) = await _billPayService.RemoveBill(id);
        var bills = await _billPayService.GetBills(loggedCustomerId.Value);
        
        var model = new BillPayListViewModel
        {
            Bills = bills.Select(b => new BillPayViewModel
            {
                BillPayID = b.BillPayID,
                AccountNumber = b.AccountNumber,
                PayeeID = b.PayeeID,
                Amount = b.Amount,
                BillPeriod = b.BillPeriod,
                ScheduleTimeUtc = b.ScheduleTimeUtc,
                Status = b.Status
            }).ToList(),

            Message = msg,
            IsSuccess = success
        };

        return View("Index", model);
    }

    [HttpGet]
    public async Task<IActionResult> Retry(int id)
    {
        var loggedCustomerId = _authService.CurrentCustomerId();
        
        var (success, msg) = await _billPayService.UpdateBillStatus(id, BillStatus.Pending);
        
        var bills = await _billPayService.GetBills(loggedCustomerId.Value);
        
        var model = new BillPayListViewModel
        {
            Bills = bills.Select(b => new BillPayViewModel
            {
                BillPayID = b.BillPayID,
                AccountNumber = b.AccountNumber,
                PayeeID = b.PayeeID,
                Amount = b.Amount,
                BillPeriod = b.BillPeriod,
                ScheduleTimeUtc = b.ScheduleTimeUtc,
                Status = b.Status
            }).ToList(),

            Message = "",
            IsSuccess = success
        };
        
        return View("Index", model);
    }
}