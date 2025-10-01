using CustomerPortal.Models;

namespace CustomerPortal.Data.Repository;

public interface IAccountRepository
{
    Task<List<Account>> GetCustomerAccounts(int customerId);
    Task<List<Account>> GetBankAccounts();
    Task<Account?> GetByAccNumber(int accountNumber);
    Task UpdateAccount(Account account);
}