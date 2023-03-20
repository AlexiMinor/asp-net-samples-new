using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace AspNetSamples.Mvc.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class TestMiddleware
    {
        private readonly RequestDelegate _next;

        public TestMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            using (var tw = new StreamWriter("C:\\Users\\AlexiMinor\\Desktop\\123\\file.txt",
                       Encoding.UTF8, 
                       new FileStreamOptions {Mode = FileMode.Append, 
                           Access = FileAccess.Write}))
            {
                var req = httpContext.Request;
                await tw.WriteLineAsync($"{DateTime.Now:R} {req.Method} - {req.Scheme}://{req.Host}{req.Path}");
            }

            httpContext.Response.Headers.Add("Hello", new StringValues("Hello world"));
            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class TestMiddlewareExtensions
    {
        public static IApplicationBuilder UseTestMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TestMiddleware>();
        }
    }
}
