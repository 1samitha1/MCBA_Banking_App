using CustomerPortal.Models;

namespace CustomerPortal.ViewModel.Statement;

public class SelectStatementVm
{
    public int? SelectedAccountNumber { get; set; }

    // Populate for the dropdown
    public List<Account> Accounts { get; set; } = new List<Account>();

    // Optional message area
    public string? Error { get; set; }
}