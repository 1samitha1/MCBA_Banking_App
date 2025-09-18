using Microsoft.AspNetCore.Mvc;
namespace CustomerPortal.Controllers;

public class LoginController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult HandleLogin(string loginId, string password)
    { 
        // check inputs
        if (string.IsNullOrEmpty(loginId) || string.IsNullOrEmpty(password))
        {
            // show error and return
            return View("Index");
        }
        
       // get login details
       // verify the hash
       // perform login
       return RedirectToAction("Index", "Home");
    }
}