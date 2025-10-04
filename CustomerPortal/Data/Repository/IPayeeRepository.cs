using CustomerPortal.Models;

namespace CustomerPortal.Data.Repository;

public interface IPayeeRepository
{
    Task<List<Payee>> GetPayees();
}