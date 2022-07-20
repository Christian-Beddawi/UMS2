using Microsoft.AspNetCore.Mvc.Filters;
using UMS.Application.Common;

namespace UMS.WebAPI.Filters;

public class UserAuthorizationFilter : Attribute, IAuthorizationFilter
{
    private readonly ICommonServices _common;
    public UserAuthorizationFilter(ICommonServices common)
    {
        _common = common;
    }
    
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var userId = context.HttpContext.Request.Headers["UserId"].First();
        Console.WriteLine("In the filter");
        Console.WriteLine("User Id  ::::  " + userId);
        //var role = _common.GetRole(userId);
    }
}