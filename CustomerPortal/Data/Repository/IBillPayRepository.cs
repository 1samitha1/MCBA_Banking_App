using CustomerPortal.Models;

namespace CustomerPortal.Data.Repository;

public interface IBillPayRepository
{
    Task<IEnumerable<BillPay>> GetBillsForCustomer(int customerId);
}