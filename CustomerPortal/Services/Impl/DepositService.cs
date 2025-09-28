using CustomerPortal.Data;
using CustomerPortal.Data.Repository;
using CustomerPortal.Models;
using CustomerPortal.Utility;
using CustomerPortal.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace CustomerPortal.Services.Impl;

public class DepositService : IDepositService
{
    private readonly McbaContext _db;
    public DepositService(McbaContext db) => _db = db;
    

    public async Task<(bool ok, string? ErrorMessage)> DepositAsync(DepositViewModel model, int customerId)
    {
        if (model.Amount < 0) 
            return (false,"Deposit amout shoul more than 0");
        
        //verify account
        var account = await _db.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == model.AccountNumber);
        if (account == null)
            return (false,"Account not found");

        var transaction = await _db.Database.BeginTransactionAsync();
        try
        {
            account.Balance += model.Amount;
            _db.Transactions.Add(new Transaction()
            {
                AccountNumber = model.AccountNumber,
                Amount = model.Amount,
                TransactionType = TransactionType.Deposit,
                TransactionTimeUtc = DateTime.UtcNow,
                Comments = model.Comment
            });
            await _db.SaveChangesAsync();
            await transaction.CommitAsync();
            return (true, null);

        }
        catch (DbUpdateConcurrencyException)
        {
            await transaction.RollbackAsync();
            return (false, "Concurrency error");
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            return (false, "Unexpected error during deposit");
        }
        
    }
}