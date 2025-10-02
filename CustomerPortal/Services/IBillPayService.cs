using CustomerPortal.Models;

namespace CustomerPortal.Services;

public interface IBillPayService
{
    Task<IEnumerable<BillPay>> GetBills(int customerID);
}