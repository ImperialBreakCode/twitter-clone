using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace TwitterUni.Filters
{
    public class SetupUserFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string? setUser = context.HttpContext.Session.GetString("setUser");
            
            if (!(setUser is not null && JsonConvert.DeserializeObject<bool>(setUser)))
            {
                context.Result = new RedirectToActionResult("Setup", "Auth", new { area = "Account" });
            }
        }
    }
}
