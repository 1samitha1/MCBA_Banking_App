using CustomerPortal.Models;

namespace CustomerPortal.Data.Repository;

public interface IAccountRepository
{
    Task<List<Account>> GetCustomerAccounts(int customerId);
    Task<Account?> WithdrawFunds(decimal amount, int accountNumber);
}