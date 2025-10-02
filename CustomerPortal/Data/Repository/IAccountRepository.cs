using CustomerPortal.Models;
using CustomerPortal.Utility;

namespace CustomerPortal.Data.Repository;

public interface IAccountRepository
{
    Task<List<Account>> GetCustomerAccounts(int customerId);
    Task<List<Account>> GetBankAccounts();
    Task UpdateAccount(Account account);
    Task<Account?> GetByAccNumber(int accountNumber);
    Task<Account?> GetAccountAsync(int accountNumber);

}