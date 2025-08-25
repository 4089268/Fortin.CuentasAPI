using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace Fortin.CuentasAPI.Data;

[AttributeUsage(AttributeTargets.All)]
public class AuthAttribute() : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var authOptions = context.HttpContext.RequestServices.GetRequiredService<IOptions<AuthSettings>>();
        var authToken = authOptions.Value.Token;

        // * get the token
        var authHeader = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
        {
            context.Result = new JsonResult(new { message = "Unauthorized" })
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };
            return;
        }
        var token = authHeader.Substring("Bearer ".Length).Trim();

        // Validate token
        if (token != authToken)
        {
            context.Result = new JsonResult(new { message = "Unauthorized" })
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };
            return;
        }

    }
}
