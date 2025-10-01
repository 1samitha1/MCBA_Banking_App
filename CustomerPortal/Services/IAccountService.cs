using CustomerPortal.Models;

namespace CustomerPortal.Services;

public interface IAccountService
{
    Task<(Account? Account,string Message)> WithdrawFunds(decimal amount, int accountNumber);
    Task<(Account Source, Account Destination, string Message)?> TransferFunds(decimal amount, decimal amountToDebit, int sourceAccNumber, int destinationAccNumber);
}