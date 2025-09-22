using CustomerPortal.Models;

namespace CustomerPortal.Data.Repository;

public interface ILoginRepository
{
    Task<Login?> GetLoginIdAsync(string loginId);
}