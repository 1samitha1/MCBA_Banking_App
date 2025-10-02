using CustomerPortal.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerPortal.Data.Repository.Impl;

public class BillPayRepository: IBillPayRepository
{
    private readonly McbaContext _db;
    
    public BillPayRepository(McbaContext db) => _db = db;
    
    public async Task<IEnumerable<BillPay>> GetBillsForCustomer(int customerId)
    {
        return await _db.BillPay
            .Include(b => b.Account)
            .Include(b => b.Payee)
            .Where(b => b.Account.CustomerID == customerId)
            .ToListAsync();
    }
}