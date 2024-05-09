namespace WholeSaler.Web.Helpers.CookieHelper
{
    public static class CookieHelper
    {
        public static string GetAccessToken(this HttpContext httpContext)
        {
            return httpContext.Request.Cookies["access_token"];
        }
    }
}
