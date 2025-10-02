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

    // withdraw funds from the selected account
    public async Task<(Account? Account,string Message)> WithdrawFunds(decimal amount, int accountNumber)
    {
        var acc = await _db.Accounts.FindAsync(accountNumber);

        if (acc == null) {
            return (null, "Withdrawal Failed! Account not found");
        }

        // check if account has enough balance to process withdrawal, only for saving accounts
        if (acc.AccountType == AccountType.S && acc.Balance < amount) {
            return (null, "Withdrawal Failed! Insufficient funds to withdraw.");
        }
        
        // update account
        acc.Balance -= amount;
        await _db.SaveChangesAsync();
        return (acc, "Withdrawal Successful");
    }
    
    public async Task<List<Account>> GetBankAccounts()
    {
        return await _db.Accounts
            .ToListAsync();
    }

    public async Task<(Account Source, Account Destination, string Message)?> TransferFunds(decimal amount, int sourceAccountNumber, int destinationAccountNumber)
    {
        var sourceAcc = await _db.Accounts.FindAsync(sourceAccountNumber);
        var destAcc = await _db.Accounts.FindAsync(destinationAccountNumber);
        
        if (sourceAcc == null || destAcc == null) {
            return (null, null, "Transfer failed! Accounts not found");
        }
        
        // check if account has enough balance to process withdrawal
        if (sourceAcc.Balance < amount) {
            return (null, null, "Transfer failed! Insufficient funds available in account for make a transfer.");
        }
        
        // update both accounts
        sourceAcc.Balance -= amount;
        destAcc.Balance += amount;
        await _db.SaveChangesAsync();
        return (Source: sourceAcc, Destination: destAcc, Message: "Transfer Successful!"); 
    }

    public async Task<Account?> GetAccountAsync(int accountNumber)
    {
        return await _db.Accounts.Where(account => account.AccountNumber == accountNumber)
            .FirstOrDefaultAsync();
    }
}