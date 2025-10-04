namespace CustomerPortal.ViewModel.BillPay;

public class BillPayListViewModel
{
    public List<BillPayViewModel> Bills { get; set; } = new();
    public string? Message { get; set; }
    public bool IsSuccess { get; set; }
}