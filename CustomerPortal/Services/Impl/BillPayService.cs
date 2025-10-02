using CustomerPortal.Data.Repository;
using CustomerPortal.Models;

namespace CustomerPortal.Services.Impl;

public class BillPayService: IBillPayService
{
    private readonly IBillPayRepository _billPayRepository;
    
    public BillPayService(IBillPayRepository billPayRepository)
    {
        _billPayRepository =  billPayRepository;
    }

    public async Task<IEnumerable<BillPay>> GetBills(int customerId)
    {
        return await _billPayRepository.GetBillsForCustomer(customerId);
    }
}