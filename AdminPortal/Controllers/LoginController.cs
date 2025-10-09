using Microsoft.AspNetCore.Mvc;

namespace AdminPortal.Controllers;

public class LoginController: Controller
{
    private readonly HttpClient _client;
    
    public LoginController(IHttpClientFactory clientFactory)
    {
        _client = clientFactory.CreateClient("api");
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult HandleLogin()
    {
        return View();
    }
}