using Microsoft.AspNetCore.Mvc;

namespace CustomerPortal.Controllers;

public class TransferController : Controller
{
    public TransferController()
    {
        
    }
    
    public async Task<IActionResult> Index() {
        var loggedCustomerId = HttpContext.Session.GetInt32("CustomerID");

        // no customer logged in
        if (loggedCustomerId == null) {
            return RedirectToAction("Index", "Home");
        }

        // var accountsList = await accountRepository.GetCustomerAccounts(loggedCustomerId.Value);
        //
        // var model = new WithdrawViewModel() { Accounts = accountsList };
        
        return View();
    }
}