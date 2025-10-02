using CustomerPortal.Utility;

namespace CustomerPortal.ViewModel.Statement;

public class TransactionRowViewModel
{
    public int TransactionID { get; init; }
    public TransactionType TransactionType { get; init; }
    public int AccountNumber { get; init; }
    public int? DestinationAccountNumber { get; init; }
    public decimal Amount { get; init; }
    public DateTime TransactionTimeUtc { get; init; }
    public string? Comment { get; init; }
    
    
    public string TransactionTypeLabel => TransactionType switch
    {
        TransactionType.Deposit       => "Deposit",
        TransactionType.Withdraw    => "Withdrawal",
        TransactionType.Transfer      => "Transfer",
        TransactionType.ServiceCharge => "Service charge",
        TransactionType.BIllPay      => "Bill payment",
        _ => TransactionType.ToString()
    };
}