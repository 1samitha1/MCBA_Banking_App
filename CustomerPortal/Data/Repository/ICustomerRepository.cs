using CustomerPortal.Models;

namespace CustomerPortal.Data.Repository;

public interface ICustomerRepository
{
    Task<Customer> GetCustomer(int customerId);
    Task<bool> UpdateCustomer(int customerId, Customer customer);
}