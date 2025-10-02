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
    public async Task<IActionResult> HandleStatement(SelectStatementVm vm, int pageNum =1)
    {
        var customerId = _authService.CurrentCustomerId();
        if (customerId == null) return RedirectToAction("Index", "Login");

        if (!vm.SelectedAccountNumber.HasValue)
        {
            vm.Accounts = await _accountRepository.GetCustomerAccounts(customerId.Value);
            return View("Index", vm);
        }
        var accountNumber = vm.SelectedAccountNumber.Value;
        
        var acc = _accountRepository.GetAccountAsync(accountNumber).Result;
        if (acc is null || acc.CustomerID != customerId) return NotFound();
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
        
        var (rows, total) = await _transactionRepository.GetPagedTransactions(accountNumber, pageNum,4);
        SingleStatementPageViewModel pageViewModel = new SingleStatementPageViewModel
        {
            AccountNumber = acc.AccountNumber,
            AccountType = accType,
            CurrentBalance = acc.Balance,
            AvailableBalance = acc.Balance,
            PageNumber = pageNum,
            PageSize = 4,
            TotalTransactions = total,
            TransactionRows = rows?.ToList() ?? new ()
        };
        return View(viewName: "Statement",pageViewModel);
        
    }
}