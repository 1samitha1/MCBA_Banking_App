
using CustomerPortal.Models;

namespace AdminApi.Data.Repository;

public interface IPayeeRepository
{
    Task<List<Payee>> GetAllPayeesAsync(string? postcode, CancellationToken ct = default);
    Task<Payee?> GetPayeeAsync(int payeeId, CancellationToken ct = default);
    Task UpdatePayeeAsync(Payee payee, CancellationToken ct  = default);
}