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
            
}