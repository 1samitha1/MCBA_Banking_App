using CustomerPortal.Models;
using CustomerPortal.Utility;

namespace CustomerPortal.Data.Repository;

public interface ITransactionRepository
{
    Task<bool> CreateTransaction(Transaction transaction);
    Task<int> GetTransactionCount(int accountId, TransactionType transactionType);
}