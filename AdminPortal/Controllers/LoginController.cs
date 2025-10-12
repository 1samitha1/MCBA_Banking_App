using System.Security.Claims;
using AdminPortal.Services;
using AdminPortal.Utility;
using AdminPortal.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminPortal.Controllers;
[AllowAnonymous]
public class LoginController: Controller
{
    private readonly IApiAuthClient  _apiAuth;
    
    public LoginController(IApiAuthClient  apiAuth)
    {
        _apiAuth = apiAuth;
    }

    [HttpGet]
    public IActionResult Index(string? returnUrl = null)
    {
        return View(new AdminLoginViewModel{ReturnUrl = returnUrl});
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> HandleLogin(AdminLoginViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid) return View(model);
        var (ok, token, role, expireAt, error) = await _apiAuth.LoginAsync(model.LoginID,model.Password,ct);

        if (!ok || token == null)
        {
            ModelState.AddModelError(string.Empty, error ?? "Login failed try again!");
            return View(model);
        }
        
        //Log-in cookie
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, model.LoginID),
            new Claim(ClaimTypes.Role, string.IsNullOrWhiteSpace(role) ? "admin" : role),
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity),
            new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30) // fixed lifetime
            });
        //Save Admin API JWT in Session for subsequent API calls
        HttpContext.Session.SetString(SessionBearerTokenHandler.SessionTokenKey, token);
        
        if (!string.IsNullOrWhiteSpace(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
            return Redirect(model.ReturnUrl);
        
        return RedirectToAction("Index", "Home");
    }
}