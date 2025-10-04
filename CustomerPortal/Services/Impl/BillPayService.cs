using CustomerPortal.Data.Repository;
using CustomerPortal.Models;
using CustomerPortal.Utility;

namespace CustomerPortal.Services.Impl;

public class BillPayService: IBillPayService
{
    private readonly IBillPayRepository _billPayRepository;
    private readonly IPayeeRepository _payeeRepository;
    
    public BillPayService(IBillPayRepository billPayRepository, IPayeeRepository payeeRepository)
    {
        _billPayRepository =  billPayRepository;
        _payeeRepository = payeeRepository;
    }

    public async Task<IEnumerable<BillPay>> GetBills(int customerId)
    {
        return await _billPayRepository.GetBillsForCustomer(customerId);
    }

    public async Task<IEnumerable<Payee>> GetPayees()
    {
        return await _payeeRepository.GetPayees();
    }

    public async Task<(bool success, List<BillPay> bills, string Message)> CreateBill(int customerId, BillPay billPay)
    {
        var createResult = await _billPayRepository.CreateBill(billPay);

        if (!createResult)
        {
            return (false, [], "Bill creation failed");
        }
        
        var bills = await _billPayRepository.GetBillsForCustomer(customerId);
        
        return (createResult, bills, "Bill creation succeeded");
        
    }

    public async Task<(bool success, string msg)> RemoveBill(int billPayId)
    {
        var removeRes = await _billPayRepository.RemoveBill(billPayId);

        if (!removeRes)
        {
            return (false,"Bill removal failed");
        }
        
        return (true, "Bill removal succeeded");
    }

    public async Task<(bool success, string msg)> UpdateBillStatus(int billPayId, BillStatus status)
    {
        var updateRes = await _billPayRepository.UpdateBillStatus(billPayId, status);
        
        if (!updateRes)
        {
            return (false,"Bill removal failed");
        }
        
        return (true, "Bill removal succeeded");
    }

    //processing Due bills for background service
    public async Task ProcessDueBills()
    {
        await _billPayRepository.ProcessDueBills();
    }
}