using CustomerPortal.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerPortal.Data.Repository.Impl;

public class LoginRepository : ILoginRepository
{
    private readonly McbaContext _db;
    public LoginRepository(McbaContext db) => _db = db;

    public Task<Login?> GetLoginIdAsync(string loginId)
        => _db.Logins.Include(l => l.Customer)
            .SingleOrDefaultAsync(l => l.LoginID == loginId);

    public async Task<Login?> GetLoginRecord(int customerId)
    {
       return await _db.Logins.FirstOrDefaultAsync(l => l.CustomerID == customerId);
    }
    public async Task<(bool result, string msg)> ChangePassword(int customerId, string newPasswordHash)
    {
        var loginRecord = await GetLoginRecord(customerId);
        
        // if all matches, updating the password
        loginRecord.PasswordHash = newPasswordHash;
        _db.Logins.Update(loginRecord);
        await _db.SaveChangesAsync();

        return (true, "Password changed successfully.");
    }
}