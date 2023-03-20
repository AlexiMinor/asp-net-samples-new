using Microsoft.AspNetCore.Mvc.Filters;

namespace AspNetSamples.Mvc.Filters;

public class MyCustomActionFilter : Attribute, IActionFilter
{

    //public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    //{
    //    context.HttpContext.Response.Cookies.Append("LastVisit", DateTime.Now.ToString("R"));
    //    await next();
    //}

    public void OnActionExecuting(ActionExecutingContext context)
    {
        context.HttpContext.Response.Cookies.Append("LastVisit", DateTime.Now.ToString("R"));
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        context.HttpContext.Response.Cookies.Append("LastVisitIsSucceed", "true");
    }
}