using CustomerPortal.Data;
using CustomerPortal.Data.Repository;
using CustomerPortal.Models;
using CustomerPortal.Utility;

namespace CustomerPortal.Services.Impl;
public class AccountService: IAccountService
{
    private readonly McbaContext _db;
    private readonly IAccountRepository _accountRepository;

    public AccountService(McbaContext db, IAccountRepository accRepository)
    {
        _db = db;
        _accountRepository = accRepository;
    }
    
    public async Task<(Account? Account, string Message)> WithdrawFunds(decimal amount, int accountNumber)
    {
        var acc = await _accountRepository.GetByAccNumber(accountNumber);

        if (acc == null)
            return (null, "Withdrawal Failed! Account not found");

        if (acc.AccountType == AccountType.S && acc.Balance < amount)
            return (null, "Withdrawal Failed! Insufficient funds to withdraw.");

        acc.Balance -= amount;
        await _accountRepository.UpdateAccount(acc);

        return (acc, "Withdrawal Successful");
    }
    
    public async Task<(Account Source, Account Destination, string Message)?> TransferFunds(
        decimal amount, decimal amountToDebit, int sourceAccountNumber, int destinationAccountNumber)
    {
        // Load accounts
        var sourceAcc = await _accountRepository.GetByAccNumber(sourceAccountNumber);
        var destAcc = await _accountRepository.GetByAccNumber(destinationAccountNumber);

        if (sourceAcc == null || destAcc == null)
            return (null, null, "Transfer failed! Accounts not found");

        // Check balances
        if (amount <= 0)
            return (null, null, "Transfer failed! Amount must be greater than zero.");

        if (sourceAcc.Balance < amount)
            return (null, null, "Transfer failed! Insufficient funds available in account.");
        
        sourceAcc.Balance -= amountToDebit;
        destAcc.Balance += amount;

        // Save changes for accounts
        await _accountRepository.UpdateAccount(sourceAcc);
        await _accountRepository.UpdateAccount(destAcc);

        return (sourceAcc, destAcc, "Transfer Successful!");
    }
}