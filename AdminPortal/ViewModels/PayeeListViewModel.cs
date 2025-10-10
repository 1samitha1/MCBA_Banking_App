namespace AdminPortal.ViewModels;

public class PayeeListViewModel
{
    public string? PostCodeFilter { get; set; }

    public List<PayeeViewModel> Payees { get; set; } = new List<PayeeViewModel>();
    public List<string?> AllPostalCodes { get; set; } = new List<string>();
    public string? SelectedPostalCode { get; set; }
    public PayeeViewModel? SelectedPayee { get; set; } 
    
    public PayeeViewModel? NewPayee { get; set; } = new PayeeViewModel();
    
}