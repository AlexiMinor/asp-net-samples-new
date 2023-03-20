using AspNetSamples.Abstractions.Services;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AspNetSamples.Mvc.Filters;

public class MyCustomResourceFilter : Attribute, IAsyncResourceFilter
{
    private readonly IArticleService _articleService;

    public MyCustomResourceFilter(IArticleService articleService)
    {
        _articleService = articleService;
    }

    public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
    {
        var articlesCount = await _articleService.GetTotalArticlesCountAsync();
        context.RouteData.Values.Add("artCount", articlesCount);
        await next();
    }
}