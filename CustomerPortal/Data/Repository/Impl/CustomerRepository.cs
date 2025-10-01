using CustomerPortal.Models;

namespace CustomerPortal.Data.Repository.Impl;

public class CustomerRepository: ICustomerRepository
{
    private readonly McbaContext _db;
    
    public CustomerRepository(McbaContext db) => _db = db;

    public async Task<Customer> GetCustomer(int customerId)
    {
        return await _db.Customers.FindAsync(customerId);
    }

    public async Task<bool> UpdateCustomer(int customerId, Customer customer)
    {
        _db.Customers.Update(customer);
        await _db.SaveChangesAsync();
        return true;
    }
}