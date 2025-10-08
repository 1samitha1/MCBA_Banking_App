using CustomerPortal.Models;
using CustomerPortal.Utility;
using Microsoft.EntityFrameworkCore;

namespace CustomerPortal.Data.Repository.Impl;

public class BillPayRepository: IBillPayRepository
{
    private readonly McbaContext _db;
    private readonly ITransactionRepository _transactionRepository;
    
    public BillPayRepository(McbaContext db, ITransactionRepository transactionRepository)
    {
        _db = db;
        _transactionRepository = transactionRepository;
    }

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

    public async Task<bool> UpdateBillStatus(int billPayId, BillStatus status)
    {
        var billPay = await _db.BillPay.FindAsync(billPayId);
        if (billPay == null) return false;

        billPay.Status = status;
        billPay.ErrorMessage = null;

        await _db.SaveChangesAsync();
        return true;
    }
    
    public async  Task ProcessDueBills()
    {
        var now = DateTime.Now;;
        var dueBills = await _db.BillPay
            .Where(b => b.Status == BillStatus.Pending && b.ScheduleTimeUtc <= now)
            .ToListAsync();

        foreach (var bill in dueBills)
        {
            // check if there is enough balance for deduction
            try
            {
                var account = await _db.Accounts.FindAsync(bill.AccountNumber);
                if (account != null && account.Balance >= bill.Amount)
                {
                    account.Balance -= bill.Amount;
                    bill.Status = BillStatus.Success;
                    bill.ErrorMessage = null;
                    
                    if (bill.BillPeriod == BillPeriod.Monthly)
                    {
                        // Schedule for next month
                        bill.ScheduleTimeUtc = bill.ScheduleTimeUtc.AddMonths(1);
                        // set status to pending for next bill
                        bill.Status = BillStatus.Pending; 
                    }
                    
                    // if bill pay is processing, creating transction for the bill
                    var billPayTransaction = new Transaction
                    {
                        AccountNumber = account.AccountNumber,
                        TransactionType = TransactionType.BIllPay,
                        Amount = bill.Amount,
                        TransactionTimeUtc = DateTime.UtcNow
                    };

                    await _transactionRepository.CreateTransaction(billPayTransaction);
                }
                else
                {
                    // store error msg and change status to failed
                    bill.Status = BillStatus.Failed;
                    bill.ErrorMessage = "Insufficient balance";
                }
            }
            catch (Exception ex)
            {
                bill.Status = BillStatus.Failed;
                bill.ErrorMessage = "Bill payment Failed: " + ex.Message;;
            }
            
            await _db.SaveChangesAsync();
        }
    }
}