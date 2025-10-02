using CustomerPortal.Models;
using CustomerPortal.Utility;
using CustomerPortal.ViewModel;
using CustomerPortal.ViewModel.Statement;
using Microsoft.EntityFrameworkCore;

namespace CustomerPortal.Data.Repository.Impl;

public class TransactionRepository: ITransactionRepository
{
    private readonly McbaContext _db;
    private readonly IAccountRepository _accountRepository;

    public TransactionRepository(McbaContext db, IAccountRepository accountRepository)
    {
        _db = db;
        _accountRepository = accountRepository;
    } 
    
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
            .Where(trans => trans.AccountNumber == accountNumber 
                            && trans.TransactionType == transactionType
                            && (transactionType != TransactionType.Transfer || trans.DestinationAccount != null)) 
            .CountAsync();
    }

    public async Task<(IReadOnlyList<TransactionRowViewModel>?, int total)> 
        GetPagedTransactions(int accountNumber, int page, int pageSize)
    {
        var account = await _accountRepository.GetAccountAsync(accountNumber);
        if (account is null)
        {
            return ( null, 0);
        }

        if(page < 1) page = 1;
        var orderedTransactions = _db.Transactions.Where(t => t.AccountNumber == accountNumber)
            .OrderByDescending(t => t.TransactionTimeUtc);

        var total = await orderedTransactions.CountAsync();

        int pageIndex = Math.Max(page, 1) - 1;
        int skip = pageIndex * pageSize;

        List<TransactionRowViewModel> rows = await orderedTransactions.Skip(skip)
            .Take(pageSize)
            .Select(t => new TransactionRowViewModel()
            {
                TransactionID = t.TransactionID,
                TransactionType = t.TransactionType,
                AccountNumber = t.AccountNumber,
                DestinationAccountNumber = t.DestinationAccountNumber,
                Amount = t.Amount,
                TransactionTimeUtc = t.TransactionTimeUtc,
                Comment = t.Comments
            }).ToListAsync();
        
        return (rows,total);
    }
}