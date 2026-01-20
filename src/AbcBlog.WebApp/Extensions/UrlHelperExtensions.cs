using AbcBlog.WebApp.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace AbcBlog.WebApp.Extensions
{
    public static class UrlHelperExtensions
    {
        public static string ResetPasswordCallbackLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
        {
            return urlHelper.Action(
                action: nameof(AuthController.ResetPassword),
                controller: "Auth",
                values: new { userId, code },
                protocol: scheme);
        }
    }
}
