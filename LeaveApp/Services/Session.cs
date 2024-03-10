using Microsoft.AspNetCore.Http;

namespace LeaveApp.Services
{
    public static class Session
    {
        public static void SetUserLoggedIn(HttpContext httpContext, bool value)
        {
            httpContext.Session.SetInt32("IsUserLoggedIn", value ? 1 : 0);
        }

        public static bool IsUserLoggedIn(HttpContext httpContext)
        {
            return httpContext.Session.GetInt32("IsUserLoggedIn") == 1;
        }
    }
}
