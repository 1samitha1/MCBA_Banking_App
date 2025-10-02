namespace CustomerPortal.ViewModel.Statement;

public class SingleStatementPageViewModel
{
    //Header
    public int AccountNumber { get; set; }
    public string AccountType { get; set; } = "";
    public decimal CurrentBalance { get; set; }
    public decimal AvailableBalance { get; set; }
    
    //A page
    public int PageNumber { get; set; }
    public int PageSize { get; set; } = 4;
    public int TotalTransactions { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalTransactions/ PageSize);
    
    //Row
    public IReadOnlyList<TransactionRowViewModel>  TransactionRows { get; set; } = new List<TransactionRowViewModel>();
    
}