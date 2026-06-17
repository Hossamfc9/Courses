using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Infrastructure.Filters;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class RoleAttribute(string requiredRole) : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var roleHeader = context.HttpContext.Request.Headers["X-User-Role"].ToString();

        if (string.IsNullOrWhiteSpace(roleHeader) || !roleHeader.Equals(requiredRole, StringComparison.OrdinalIgnoreCase))
        {
            context.Result = new ObjectResult(new 
            { 
                title = "Forbidden", 
                status = StatusCodes.Status403Forbidden, 
                detail = $"Access denied. This endpoint requires the '{requiredRole}' role." 
            })
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
            return;
        }

        await next();
    }
}
