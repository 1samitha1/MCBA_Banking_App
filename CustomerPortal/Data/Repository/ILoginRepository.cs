using CustomerPortal.Models;

namespace CustomerPortal.Data.Repository;

public interface ILoginRepository
{
    Task<Login?> GetLoginIdAsync(string loginId);
    
    Task<Login?> GetLoginRecord(int customerId);
    
    Task<(bool result, string msg)> ChangePassword(int customerId, string newPassword);
}