namespace AdminPortal.ViewModels;

public class BillPayListViewModel
{
    public List<BillPayViewModel> Items { get; set; } = new();
    public bool? FilterIsBlocked { get; set; }
}