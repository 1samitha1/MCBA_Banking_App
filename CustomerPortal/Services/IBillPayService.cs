using CustomerPortal.Models;

namespace CustomerPortal.Services;

public interface IBillPayService
{
    Task<IEnumerable<BillPay>> GetBills(int customerId);
    Task<IEnumerable<Payee>> GetPayees();
    
    Task<(bool success, List<BillPay> bills, string Message)> CreateBill(int customerId, BillPay billPay);
    
    Task<(bool success, string msg)> RemoveBill(int billPayId); 
}