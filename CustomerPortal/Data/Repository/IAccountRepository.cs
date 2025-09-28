using CustomerPortal.Models;

namespace CustomerPortal.Data.Repository;

public interface IAccountRepository
{
    Task<List<Account>> GetCustomerAccounts(int customerId);
    Task<(Account? Account,string Message)> WithdrawFunds(decimal amount, int accountNumber);
    Task<List<Account>> GetBankAccounts();
    Task<(Account Source, Account Destination, string Message)?> TransferFunds(decimal amount, int sourceAccNumber, int destinationAccNumber);
}