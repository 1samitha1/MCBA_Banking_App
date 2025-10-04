using Microsoft.EntityFrameworkCore;

namespace CustomerPortal.Data.Repository.Impl;
using CustomerPortal.Models;

public class PayeeRepository: IPayeeRepository
{
    private readonly McbaContext _db;
    
    public PayeeRepository(McbaContext db) => _db = db;

    public async Task<List<Payee>> GetPayees()
    {
        return await _db.Payees
            .ToListAsync();
    }
}