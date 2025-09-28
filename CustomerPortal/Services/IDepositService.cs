using CustomerPortal.ViewModel;

namespace CustomerPortal.Services;

public interface IDepositService
{
    Task<(bool ok, string? ErrorMessage)> DepositAsync(DepositViewModel model, int customerId);
}