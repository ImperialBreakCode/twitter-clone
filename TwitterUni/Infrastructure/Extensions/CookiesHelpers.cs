using Newtonsoft.Json;

namespace TwitterUni.Infrastructure.Extensions
{
    public static class CookiesHelpers
    {
        public static void AddAuthHelperCookie(this IResponseCookies responseCookies, string name, object? value)
        {
            CookieOptions options = new CookieOptions();
            options.HttpOnly = true;

            responseCookies.Append(name, JsonConvert.SerializeObject(value), options);
        }
    }
}
