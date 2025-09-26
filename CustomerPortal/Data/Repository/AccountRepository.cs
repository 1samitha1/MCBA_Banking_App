using CustomerPortal.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerPortal.Data.Repository;

public class AccountRepository:  IAccountRepository
{
    private readonly McbaContext _db;
    
    public AccountRepository(McbaContext db) => _db = db;

    // find accounts for the customer
    public async Task<List<Account>> GetCustomerAccounts(int customerId)
    {
        return await _db.Accounts
            .Where(a => a.CustomerID == customerId)
            .ToListAsync();
    }

    // withdraw funds from the selected account
    public async Task<Account?> WithdrawFunds(decimal amount, int accountNumber)
    {
        var acc = await _db.Accounts.FindAsync(accountNumber);

        if (acc == null) {
            return null;
        }

        // check if account has enough balance to process withdrawal
        if (acc.Balance < amount) {
            throw new InvalidOperationException("Insufficient funds to withdraw.");
        }
        
        acc.Balance -= amount;
        await _db.SaveChangesAsync();
        return acc;
    }
}