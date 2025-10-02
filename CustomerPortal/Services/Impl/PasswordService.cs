using CustomerPortal.Data.Repository;
using SimpleHashing.Net;

namespace CustomerPortal.Services.Impl;

public class PasswordService : IPasswordService
{
    private readonly ISimpleHash _hasher = new SimpleHash();
    private readonly ILoginRepository _loginRepository;

    public PasswordService(ILoginRepository loginRepository)
    {
        _loginRepository = loginRepository;
    }

    public bool Verify(string rawPassword, string storedHash)
        => _hasher.Verify(rawPassword, storedHash);

    public string Hash(string rawPassword) 
        => _hasher.Compute(rawPassword);

    public async Task<(bool result, string msg)> ChangePassword(int customerId, string oldPassword, string newPassword)
    {
        var newPasswordHash = Hash(newPassword.Trim());
        
        // get login record for the customer
        var loginRecord = await _loginRepository.GetLoginRecord(customerId);
        
        if (loginRecord == null)
        {
            return (false, "Customer login record not found.");
        }
        
        var verifyCurrentPassword = Verify(oldPassword, loginRecord.PasswordHash);
        
        // Check if the current password matched with db record
        if (!verifyCurrentPassword)
        {
            return (false, "Current password is incorrect.");
        }
        
        // update the password
        return await _loginRepository.ChangePassword(customerId, newPasswordHash);
    }
    
}
