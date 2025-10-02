using CustomerPortal.Models;

namespace CustomerPortal.Services;

public interface ICustomerService
{
    Task<Customer> GetCustomer(int customerId);
    Task<Customer> UpdateCustomer(int customerId, Customer customer);
}