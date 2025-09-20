namespace CustomerPortal.Utility;

using SimpleHashing.Net;

public class PasswordService
{
    private readonly ISimpleHash _hasher = new SimpleHash();

    public bool Verify(string rawPassword, string storedHash)
        => _hasher.Verify(rawPassword, storedHash);
}
