using CustomerPortal.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerPortal.Data.Repository.Impl;

public class BillPayRepository: IBillPayRepository
{
    private readonly McbaContext _db;
    
    public BillPayRepository(McbaContext db) => _db = db;
    
    public async Task<List<BillPay>> GetBillsForCustomer(int customerId)
    {
        return await _db.BillPay
            .Include(b => b.Account)
            .Include(b => b.Payee)
            .Where(b => b.Account.CustomerID == customerId)
            .ToListAsync();
    }

    public async Task<bool> CreateBill(BillPay billPay)
    {
        await _db.BillPay.AddAsync(billPay);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<BillPay> FindBill(int billPayId)
    {
        var bill = await _db.BillPay.FindAsync(billPayId);
        if (bill == null)
            return null;
        return bill;
    }

    public async Task<bool> RemoveBill(int billPayId)
    {
        var bill = await FindBill(billPayId);

        _db.BillPay.Remove(bill);
        await _db.SaveChangesAsync();
        return true;
    }
}