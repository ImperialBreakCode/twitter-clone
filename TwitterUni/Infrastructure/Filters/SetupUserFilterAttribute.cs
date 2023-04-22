using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace TwitterUni.Infrastructure.Filters
{
    public class SetupUserFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string? setUser = context.HttpContext.Request.Cookies["SetUser"];

            if (!(setUser is not null && JsonConvert.DeserializeObject<bool>(setUser)))
            {
                context.Result = new RedirectToActionResult("Setup", "Auth",
                    new
                    {
                        area = "Account",
                        Id = context.HttpContext.User.Identity.Name
                    });
            }
        }
    }
}
