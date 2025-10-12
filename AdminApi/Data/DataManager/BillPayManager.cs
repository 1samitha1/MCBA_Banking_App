using AdminApi.Data.Repository;
using AdminApi.Dtos;
using CustomerPortal.Data;
using CustomerPortal.Models;
using CustomerPortal.Utility;
using Microsoft.EntityFrameworkCore;

namespace AdminApi.Data.DataManager;

public class BillPayManager : IBillPayRepository
{
    private readonly McbaContext _db;

    public BillPayManager(McbaContext db)
    {
        _db = db;
    }

    public async Task<List<BillPayDto>> GetAllAsync(bool? isBlocked, CancellationToken ct = default)
    {
        var query = _db.BillPay.AsQueryable();
        
        if (isBlocked is not null)
        {
            query = query.Where(b => b.IsBlocked == isBlocked);
        }

        return await query
            .OrderBy(b => b.BillPayID)
            .Select(b => new BillPayDto(
                b.PayeeID,
                b.AccountNumber,
                b.PayeeID,
                b.Payee.Name,
                b.Amount,
                b.ScheduleTimeUtc,
                b.BillPeriod,
                b.IsBlocked))
            .ToListAsync(ct);
    }

    public async Task<BillPay?> GetAsync(int id, CancellationToken ct = default)
    {
        return await _db.BillPay.SingleOrDefaultAsync(b=> b.BillPayID == id, ct);
    }

    public async Task SetBlockedAsync(int id, bool block, CancellationToken ct = default)
    {

        var entity = await _db.BillPay.SingleOrDefaultAsync(b => b.BillPayID == id, ct);
        if (entity == null)
        {
            throw new KeyNotFoundException($"BillPay {id} not found");
        }

        entity.IsBlocked = block;
        await _db.SaveChangesAsync(ct);
    }
}