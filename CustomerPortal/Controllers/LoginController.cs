using Microsoft.AspNetCore.Mvc;

namespace CustomerPortal.Controllers;

public class LoginController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}