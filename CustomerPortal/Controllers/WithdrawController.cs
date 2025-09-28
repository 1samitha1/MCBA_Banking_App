using CustomerPortal.Data.Repository;
using CustomerPortal.Models;
using CustomerPortal.Utility;
using CustomerPortal.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace CustomerPortal.Controllers;

public class WithdrawController : Controller
{
    private readonly IAccountRepository accountRepository;
    private readonly ITransactionRepository transactionRepository;
    private readonly decimal withdrawFee;

    public WithdrawController(IAccountRepository accRepository, ITransactionRepository transRepository, IConfiguration configuration)
    {
        accountRepository = accRepository;
        transactionRepository = transRepository;
        withdrawFee = decimal.Parse(configuration["TransactionFees:withdrawal"]);
    }
    public async Task<IActionResult> Index() {
        var loggedCustomerId = HttpContext.Session.GetInt32("CustomerID");

        // no customer logged in
        if (loggedCustomerId == null) {
            return RedirectToAction("Index", "Home");
        }

        var accountsList = await accountRepository.GetCustomerAccounts(loggedCustomerId.Value);
        
        var model = new WithdrawViewModel() { Accounts = accountsList };
        
        return View(model);
    }
    
    [HttpPost]
    public async Task<IActionResult> Withdraw(decimal amount, int accountNumber, string comment = null) {
        try
        {
            var loggedCustomerId = HttpContext.Session.GetInt32("CustomerID");
            
            // if user not logged in or session not found, redirect to login page
            if (loggedCustomerId == null)
            {
                return RedirectToAction("Index", "Home");
            }
           
            decimal withdrawAmount = amount;
            bool isFeeAvailable = false;
            var model = new WithdrawViewModel();
            // check for the number of transactions for calculate fee
            int numOfWithdrawTrans = await transactionRepository.GetTransactionCount(accountNumber, TransactionType.Withdraw);
            // check if free transactions limit reached
            if (numOfWithdrawTrans >= 2)
            {
                withdrawAmount = amount + withdrawFee;
                isFeeAvailable = true;
            }
            
            var withdrawRes = await accountRepository.WithdrawFunds(withdrawAmount, accountNumber);

            if (withdrawRes.Account == null)
            {
                model.Message = withdrawRes.Message;
                model.IsSuccess = false;
                
                model.Accounts = await accountRepository.GetCustomerAccounts(loggedCustomerId.Value);
                return View("Index", model);
            }

            var withdrawnAccount = withdrawRes.Account;

            // create withdraw transaction
            var transaction = new Transaction
            {
                AccountNumber = withdrawnAccount.AccountNumber,
                TransactionType = TransactionType.Withdraw,
                Amount = amount,
                Comments = comment,
                TransactionTimeUtc = DateTime.UtcNow
            };

            await transactionRepository.CreateTransaction(transaction);
            
            // creating a fee transaction if there are more than 2 withdrawals available
            if (isFeeAvailable)
            {
                var feeTransaction = new Transaction
                {
                    AccountNumber = accountNumber,
                    TransactionType = TransactionType.ServiceCharge,
                    Amount = withdrawFee,
                    Comments = comment,
                    TransactionTimeUtc = DateTime.UtcNow
                };

                await transactionRepository.CreateTransaction(feeTransaction);
            }
            
            // show success message and reset inputs
            ModelState.Clear();
            
            model.Message = withdrawRes.Message;
            model.IsSuccess = true;
            model.Amount = 0;
            model.Comment = "";
            model.Accounts = await accountRepository.GetCustomerAccounts(withdrawnAccount.CustomerID);

            return View("Index", model);
        }
        catch (InvalidOperationException ex) {
            return BadRequest(new { error = ex.Message });
        }
    }
}