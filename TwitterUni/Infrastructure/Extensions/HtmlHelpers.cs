using Microsoft.AspNetCore.Mvc.Rendering;

namespace TwitterUni.Infrastructure.Extensions
{
    public static class HtmlHelpers
    {
        public static string IsActive(this IHtmlHelper htmlHelper, string controller, string action, string activeClass, string? id = null)
        {
            RouteData routeData = htmlHelper.ViewContext.RouteData;

            string routeAction = routeData.Values["action"].ToString();
            string routeController = routeData.Values["controller"].ToString();
            var routeId = routeData.Values["id"];

            bool idActive = true;
            if (id is not null && routeId is not null)
            {
                idActive = id == routeId.ToString();
            }

            var returnActive = controller == routeController && action == routeAction && idActive;

            return returnActive ? activeClass : "";
        }
    }
}
