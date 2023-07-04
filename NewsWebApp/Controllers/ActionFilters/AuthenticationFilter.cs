using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;

namespace NewsWebApp.Controllers.ActionFilters
{
    public class AuthenticationFilter:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            bool logged = context.HttpContext.Request.Cookies["token"]!=null ? true : false;
            if (!logged) {

                context.Result = new RedirectToActionResult("CustomError","Home", new { message = "Unauthorized" });

            }
        }
    }
}
