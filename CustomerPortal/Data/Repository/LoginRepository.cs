using CustomerPortal.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerPortal.Data.Repository;

public class LoginRepository : ILoginRepository
{
    private readonly McbaContext _db;
    public LoginRepository(McbaContext db) => _db = db;

    public Task<Login?> GetLoginIdAsync(string loginId)
        => _db.Logins.Include(l => l.Customer)
            .SingleOrDefaultAsync(l => l.LoginID == loginId);
}