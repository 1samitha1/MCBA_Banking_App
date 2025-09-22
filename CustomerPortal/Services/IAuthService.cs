namespace CustomerPortal.Services;

public interface IAuthService 
{
    Task<(bool ok, string? message, int? customerId, string? customerName)> 
    SignInAsync(string loginId, string password);

    void SignOut();
    bool IsSingIn();
    int? CurrentCustomerId();
    
}