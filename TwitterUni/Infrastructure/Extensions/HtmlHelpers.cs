using Microsoft.AspNetCore.Mvc.Rendering;

namespace TwitterUni.Infrastructure.Extensions
{
    public static class HtmlHelpers
    {
        public static string IsActive(this IHtmlHelper htmlHelper, string controller, string action, string activeClass)
        {
            RouteData routeData = htmlHelper.ViewContext.RouteData;

            string routeAction = routeData.Values["action"].ToString();
            string routeController = routeData.Values["controller"].ToString();

            var returnActive = controller == routeController && action == routeAction;

            return returnActive ? activeClass : "";
        }
    }
}
