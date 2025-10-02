namespace CustomerPortal.Services;

public interface IPasswordService
{
    bool Verify(string rawPassword, string storedHash);
    string Hash(string rawPassword);
    
   Task<(bool result, string msg)> ChangePassword(int customerId, string oldPassword, string newPassword);
}