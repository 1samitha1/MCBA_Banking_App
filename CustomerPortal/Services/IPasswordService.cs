namespace CustomerPortal.Services;

public interface IPasswordService
{
    bool Verify(string rawPassword, string storedHash);
    string Hash(string rawPassword);
}