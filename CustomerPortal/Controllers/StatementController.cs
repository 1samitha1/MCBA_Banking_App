using System.Diagnostics;
using CustomerPortal.Data.Repository;
using CustomerPortal.Models;
using CustomerPortal.Services;
using CustomerPortal.Utility;
using CustomerPortal.ViewModel.Statement;
using Microsoft.AspNetCore.Mvc;

namespace CustomerPortal.Controllers;

public class StatementController : Controller
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IAuthService _authService;
    public StatementController(ITransactionRepository transactionRepository, 
        IAccountRepository accountRepository, IAuthService authService)
    {
        _transactionRepository = transactionRepository;
        _accountRepository = accountRepository;
        _authService = authService;
        
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var customerId = _authService.CurrentCustomerId();
        if (customerId == null)
        {
            return RedirectToAction("Index", "Login");
        }

        var customerAccounts = await _accountRepository.GetCustomerAccounts(customerId.Value);
        SelectStatementVm vm = new SelectStatementVm()
        {
            Accounts = customerAccounts
        };
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> HandleStatement(SelectStatementVm vm)
    {
        var customerId = _authService.CurrentCustomerId();
        if (customerId == null) return RedirectToAction("Index", "Login");

        if (!vm.SelectedAccountNumber.HasValue)
        {
            vm.Accounts = await _accountRepository.GetCustomerAccounts(customerId.Value);
            return RedirectToAction(nameof(Index));
        }

        return RedirectToAction(nameof(Statement), new
        {
            accountNumber = vm.SelectedAccountNumber.Value, page = 1
        });  


    }

    [HttpGet]
    public async Task<IActionResult> Statement(int accountNumber, int page = 1)
    {
        var customerId = _authService.CurrentCustomerId();
        if (customerId == null) return RedirectToAction("Index", "Login");
        
        var acc = _accountRepository.GetAccountAsync(accountNumber).Result;
        if (acc is null || acc.CustomerID != customerId) return NotFound();
        //setting display name for current and saving accounts
        string accType = String.Empty;
        switch (acc.AccountType)
        {
            case AccountType.C:
                accType = "Current";
                break;
            case AccountType.S:
                accType = "Savings";
                break;
        }
        
        var (rows, total) = await _transactionRepository.GetPagedTransactions(accountNumber, page,4);
        SingleStatementPageViewModel pageViewModel = new SingleStatementPageViewModel
        {
            AccountNumber = acc.AccountNumber,
            AccountType = accType,
            CurrentBalance = acc.Balance,
            AvailableBalance = acc.Balance,
            PageNumber = page,
            PageSize = 4,
            TotalTransactions = total,
            TransactionRows = rows?.ToList() ?? new ()
        };
        return View(viewName: "Statement",pageViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Print(int accountNumber)
    {
        Console.WriteLine(accountNumber);
        var customerId = _authService.CurrentCustomerId();
        if (customerId == null) return RedirectToAction("Index", "Login");
        
        var acc = await _accountRepository.GetAccountAsync(accountNumber);
        if (acc is null || acc.CustomerID != customerId.Value) return NotFound();

        var accType = acc.AccountType == AccountType.C ? "Current" : "Savings";
        
        // Pull ALL transactions in one go (minimal code; reuse existing repo method)
        var (rows, total) = await _transactionRepository.GetPagedTransactions(accountNumber, page: 1, pageSize: int.MaxValue);

        var vm = new SingleStatementPageViewModel
        {
            AccountNumber = acc.AccountNumber,
            AccountType   = accType,
            CurrentBalance   = acc.Balance,
            AvailableBalance = acc.Balance,

            // For completeness; pagination isn't used on print view
            PageNumber        = 1,
            PageSize          = total == 0 ? 1 : total,
            TotalTransactions = total,
            TransactionRows   = rows?.ToList() ?? new()
        };

        return View("Print", vm);
    }
    
    
}