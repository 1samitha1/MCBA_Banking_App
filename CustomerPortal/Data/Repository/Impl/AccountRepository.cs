using CustomerPortal.Models;
using CustomerPortal.Utility;
using Microsoft.EntityFrameworkCore;

namespace CustomerPortal.Data.Repository.Impl;

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
    
    public async Task<List<Account>> GetBankAccounts()
    {
        return await _db.Accounts
            .ToListAsync();
    }
    
    // get bank account by account number
    public async Task<Account?> GetByAccNumber(int accountNumber)
    {
        return await _db.Accounts.FindAsync(accountNumber);
    }

    // update bank account
    public async Task UpdateAccount(Account account)
    {
        _db.Accounts.Update(account);
        await _db.SaveChangesAsync();
    }
}