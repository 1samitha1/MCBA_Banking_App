using AdminApi.Data.Repository;
using AdminApi.Dtos;
using CustomerPortal.Data;
using CustomerPortal.Models;
using Microsoft.EntityFrameworkCore;

namespace AdminApi.Data.DataManager;

public class PayeeManager : IPayeeRepository
{
    private readonly McbaContext _db;
    public PayeeManager(McbaContext db)
    {
        _db = db;
    }
    public async Task<List<Payee>> GetAllPayeesAsync(string? postcode, CancellationToken ct = default)=>
    
        await _db.Payees.Where(p => p.PostCode == postcode)
            .OrderBy(p => p.Name)
            .ToListAsync(ct);

    public async Task<Payee?> GetPayeeAsync(int payeeId, CancellationToken ct = default)
        => await _db.Payees.Where(p => p.PayeeID == payeeId).SingleOrDefaultAsync(ct);

    public Task UpdatePayeeAsync(Payee payee, CancellationToken ct = default)
    {
        _db.Payees.Update(payee);
        return _db.SaveChangesAsync(ct);
    } 
}