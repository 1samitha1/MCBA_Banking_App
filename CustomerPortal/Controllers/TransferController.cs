using CustomerPortal.Data.Repository;
using CustomerPortal.Models;
using CustomerPortal.Utility;
using CustomerPortal.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace CustomerPortal.Controllers;

public class TransferController : Controller
{
    private readonly IAccountRepository accountRepository;
    private readonly ITransactionRepository transactionRepository;
    private readonly decimal transferFee;
    public TransferController(IAccountRepository accRepository, ITransactionRepository transRepository, IConfiguration configuration)
    {
        accountRepository = accRepository;
        transactionRepository = transRepository;
        transferFee = decimal.Parse(configuration["TransactionFees:transfer"]);
    }
    
    public async Task<IActionResult> Index() {
        var loggedCustomerId = HttpContext.Session.GetInt32("CustomerID");

        // no customer logged in
        if (loggedCustomerId == null) {
            return RedirectToAction("Index", "Home");
        }

        // customer accounts
        var accountsList = await accountRepository.GetCustomerAccounts(loggedCustomerId.Value);
        // destination accounts
        var allAccounts = await accountRepository.GetBankAccounts();
        
        var model = new TransferViewModel() { CustomerAccounts = accountsList, DestinationAccounts = allAccounts};
        
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Transfer(decimal amount, int SourceAccNumber, int DestinationAccNumber, string comment = null) {
        try {
            var loggedCustomerId = HttpContext.Session.GetInt32("CustomerID");
            
            // if user not logged in or session not found, redirect to login page
            if (loggedCustomerId == null)
            {
                return RedirectToAction("Index", "Home");
            }
            
            decimal transferAmount = amount;
            bool isFeeAvailable = false;
            var model = new TransferViewModel();
            
            // check for the number of transactions for calculate fee
            int numOfTransferTrans = await transactionRepository.GetTransactionCount(SourceAccNumber, TransactionType.Transfer);
            // check if free transactions limit reached
            if (numOfTransferTrans >= 2)
            {
                transferAmount = amount + transferFee;
                isFeeAvailable = true;
            }
            

            // check if user try to transfer for same accounts
            if (SourceAccNumber == DestinationAccNumber)
            {
                model.Message = "Source and destination account numbers can't be the same!";
                model.IsSuccess = false;

                model.CustomerAccounts = await accountRepository.GetCustomerAccounts(loggedCustomerId.Value);
                model.DestinationAccounts = await accountRepository.GetBankAccounts();

                return View("Index", model);
            }
            
            var transferRes = await accountRepository.TransferFunds(transferAmount, SourceAccNumber, DestinationAccNumber);
            
            if (transferRes.Value.Source == null)
            {
                model.Message = transferRes.Value.Message;
                model.IsSuccess = false;
                
                model.CustomerAccounts = await accountRepository.GetCustomerAccounts(loggedCustomerId.Value);
                model.DestinationAccounts = await accountRepository.GetBankAccounts();

                return View("Index", model);
                
            }
            
            var sourceAcc = transferRes.Value.Source;
            var destAcc = transferRes.Value.Destination;
            
            // create source account transaction
            var sourceTransaction = new Transaction()
            {
                AccountNumber = sourceAcc.AccountNumber,
                TransactionType = TransactionType.Transfer,
                Amount = amount,
                DestinationAccountNumber = destAcc.AccountNumber,
                Comments = comment,
                TransactionTimeUtc = DateTime.UtcNow
            };
            
            await transactionRepository.CreateTransaction(sourceTransaction);
            
            // create source account transaction
            var destinationTransaction = new Transaction()
            {
                AccountNumber = destAcc.AccountNumber,
                TransactionType = TransactionType.Transfer,
                Amount = amount,
                Comments = comment,
                TransactionTimeUtc = DateTime.UtcNow
            };
            
            await transactionRepository.CreateTransaction(destinationTransaction);
            
            // creating a fee transaction if there are more than 2 withdrawals available
            if (isFeeAvailable)
            {
                var feeTransaction = new Transaction
                {
                    AccountNumber = SourceAccNumber,
                    TransactionType = TransactionType.ServiceCharge,
                    Amount = transferFee,
                    Comments = comment,
                    TransactionTimeUtc = DateTime.UtcNow
                };
            
                await transactionRepository.CreateTransaction(feeTransaction);
            }
            
            // after transfer successful, clear the form
            ModelState.Clear();
            
            model.Message = transferRes.Value.Message;
            model.IsSuccess = true;
            model.Amount = 0;
            model.Comment = "";
            model.CustomerAccounts = await accountRepository.GetCustomerAccounts(sourceAcc.CustomerID);
            model.DestinationAccounts = await accountRepository.GetBankAccounts();
            
            return View("Index", model);
            
        }
        catch (InvalidOperationException ex) {
            return BadRequest(new { error = ex.Message });
        }
    }
}