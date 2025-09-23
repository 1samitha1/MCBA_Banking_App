using CustomerPortal.Data.Repository;

namespace CustomerPortal.Services.Impl;

public class AuthService :IAuthService
{
    private readonly IPasswordService _passwordService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILoginRepository _loginRepository;
    
    private const string CustomerKey = "CustomerID";
    private const string CustomerNameKey = "CustomerName";
    public AuthService(IPasswordService passwordService, IHttpContextAccessor httpContextAccessor, ILoginRepository loginRepository)
    {
        _passwordService = passwordService;
        _httpContextAccessor = httpContextAccessor;
        _loginRepository = loginRepository;
    }

    public async Task<(bool ok, string? message, int? customerId, string? customerName)> 
        SignInAsync(string loginId, string password)
    {
        var login = await _loginRepository.GetLoginIdAsync(loginId);
        Console.WriteLine("Here");
        if (login == null || !_passwordService.Verify(password, login.PasswordHash))
        {
            return (false, "Invalid login or password", null, null);
        }

        var httpContextSession = _httpContextAccessor.HttpContext!.Session;
        httpContextSession.SetInt32(CustomerKey, login.CustomerID);
        httpContextSession.SetString(CustomerNameKey, login.Customer?.Name ?? "Customer");
        
        return (true, null, login.CustomerID, login.Customer?.Name);
    }

    public void SignOut() => _httpContextAccessor.HttpContext!.Session.Clear();

    public bool IsSingIn() => _httpContextAccessor.HttpContext!.Session.GetInt32(CustomerKey) is not null;
    public int? CurrentCustomerId() => _httpContextAccessor.HttpContext!.Session.GetInt32(CustomerKey);
}