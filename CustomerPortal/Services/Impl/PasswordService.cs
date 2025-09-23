using SimpleHashing.Net;

namespace CustomerPortal.Services.Impl;

public class PasswordService : IPasswordService
{
    private readonly ISimpleHash _hasher = new SimpleHash();

    public bool Verify(string rawPassword, string storedHash)
        => _hasher.Verify(rawPassword, storedHash);

    public string Hash(string rawPassword) 
        => _hasher.Compute(rawPassword);
    
}
