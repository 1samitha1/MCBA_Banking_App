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
           
            decimal withdrawAmount = amount;
            bool isFeeAvailable = false;
            // check for the number of transactions for calculate fee
            int numOfWithdrawTrans = await transactionRepository.GetTransactionCount(accountNumber, TransactionType.Withdraw);
            // check if free transactions limit reached
            if (numOfWithdrawTrans >= 2)
            {
                withdrawAmount = amount + withdrawFee;
                isFeeAvailable = true;
            }
            
            var account = await accountRepository.WithdrawFunds(withdrawAmount, accountNumber);

            if (account == null)
                return NotFound("Withdraw failed");

            // create withdraw transaction
            var transaction = new Transaction
            {
                AccountNumber = account.AccountNumber,
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
            var loggedCustomerId = HttpContext.Session.GetInt32("CustomerID");
            var model = new WithdrawViewModel();
            model.Message = "Withdrawal successful!";
            model.IsSuccess = true;
            model.Amount = 0;
            model.Comment = "";
            model.Accounts = await accountRepository.GetCustomerAccounts(account.CustomerID);

            return View("Index", model);
        }
        catch (InvalidOperationException ex) {
            return BadRequest(new { error = ex.Message });
        }
    }
}