using CustomerPortal.Data.Repository;
using CustomerPortal.Models;

namespace CustomerPortal.Services.Impl;

public class CustomerService: ICustomerService
{
    private ICustomerRepository _customerRepository;

    public CustomerService (ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<Customer> GetCustomer(int customerId)
    {
        if (customerId == null)
        {
            return null;
        }
        return await _customerRepository.GetCustomer(customerId);
    }

    public async Task<Customer> UpdateCustomer(int customerId, Customer customer)
    {
        // get customer
        var customerData = await _customerRepository.GetCustomer(customerId);

        if (customerData == null)
        {
            return null;
        }
        
        customerData.Name = customer.Name;
        customerData.TFN = customer.TFN;
        customerData.Address = customer.Address;
        customerData.City = customer.City;
        customerData.State = customer.State;
        customerData.PostCode = customer.PostCode;
        customerData.Mobile = customer.Mobile;
        
        // bind new data and update the customer
        await _customerRepository.UpdateCustomer(customerId, customerData);

        // get updated customer
        var updatedCustomer = await _customerRepository.GetCustomer(customerId);
        return updatedCustomer;
    }
}