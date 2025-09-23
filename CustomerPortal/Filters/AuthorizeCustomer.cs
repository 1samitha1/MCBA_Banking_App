using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CustomerPortal.Filters;

public class AuthorizeCustomer : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var session = context.HttpContext.Session;

        if (session.GetInt32("CustomerId") is null &&
            session.GetString("Role") != "Customer")
        {
            context.Result = new RedirectToActionResult("Index", "Login", null);
        }
    }
}