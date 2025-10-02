using CustomerPortal.Models;
using CustomerPortal.Utility;
using CustomerPortal.ViewModel;
using CustomerPortal.ViewModel.Statement;

namespace CustomerPortal.Data.Repository;

public interface ITransactionRepository
{
    Task<bool> CreateTransaction(Transaction transaction);
    Task<int> GetTransactionCount(int accountId, TransactionType transactionType);
    Task<(IReadOnlyList<TransactionRowViewModel>?, int total)> 
        GetPagedTransactions(int accountNumber, int page, int pageSize);
}