using CustomerPortal.Models;
using CustomerPortal.Utility;
using Microsoft.EntityFrameworkCore;


namespace CustomerPortal.Data.Repository;

public class TransactionRepository: ITransactionRepository
{
    private readonly McbaContext _db;
    
    public TransactionRepository(McbaContext db) => _db = db;
    
    // Create a new Transaction
    public async Task<bool> CreateTransaction(Transaction transactionItem)
    {
        await _db.Transactions.AddAsync(transactionItem);
        await _db.SaveChangesAsync();
        return true;
    }
    
     
    // Count how many trasactions available for the account based on trsaction type
    public async Task<int> GetTransactionCount(int accountNumber, TransactionType transactionType)
    {
        return await _db.Transactions
            .Where(t => t.AccountNumber == accountNumber && t.TransactionType == transactionType)
            .CountAsync();
    }
}