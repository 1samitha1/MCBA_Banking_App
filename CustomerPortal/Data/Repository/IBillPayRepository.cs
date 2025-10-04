using CustomerPortal.Models;
using CustomerPortal.Utility;

namespace CustomerPortal.Data.Repository;

public interface IBillPayRepository
{
    Task<List<BillPay>> GetBillsForCustomer(int customerId);
    
    Task<bool> CreateBill(BillPay billPay);
    
    Task<bool> RemoveBill(int billPayId);
    
    Task<bool> UpdateBillStatus(int billPayId, BillStatus status);

    Task ProcessDueBills();
}