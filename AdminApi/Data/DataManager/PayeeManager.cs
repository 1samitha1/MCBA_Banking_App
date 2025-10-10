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

    public async Task<List<Payee>> GetAllPayeesAsync(string? postcode, CancellationToken ct = default)
    {
        var query = _db.Payees.AsQueryable();

        // if filter available setting the filter
        if (!string.IsNullOrEmpty(postcode))
        {
            query = query.Where(p => p.PostCode == postcode);
        }

        return await query
            .OrderBy(p => p.Name)
            .ToListAsync(ct);
        
        // await _db.Payees.Where(p => p.PostCode == postcode)
        //     .OrderBy(p => p.Name)
        //     .ToListAsync(ct);
    }
    
    public async Task<Payee?> GetPayeeAsync(int payeeId, CancellationToken ct = default)
        => await _db.Payees.Where(p => p.PayeeID == payeeId).SingleOrDefaultAsync(ct);

    public Task UpdatePayeeAsync(Payee payee, CancellationToken ct = default)
    {
        _db.Payees.Update(payee);
        return _db.SaveChangesAsync(ct);
    }

    public async Task CreatePayeeAsync(PayeeDto payeeDto, CancellationToken ct = default)
    {
        await _db.Payees.AddAsync(new Payee()
        {
            Name = payeeDto.Name,
            Address = payeeDto.Address,
            City = payeeDto.City,
            State = payeeDto.State,
            Phone = payeeDto.Phone,
            PostCode = payeeDto.Postcode
        }, ct);
        
         await _db.SaveChangesAsync(ct);
    }
}