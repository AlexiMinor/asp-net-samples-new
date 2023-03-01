using AspNetSamples.Abstractions.Services;
using AspNetSamples.Business;
using AspNetSamples.Data;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AspNetSamples.Mvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddInMemoryCollection(new Dictionary<string, string>()
            {
                {"User1","Bob"},
                {"User2","Alice"},
            });

            builder.Services.AddDbContext<NewsAggregatorContext>(
                opt =>
            {
                var connString = builder.Configuration
                    .GetConnectionString("DefaultConnection");
                opt.UseSqlServer(connString);
            });

            builder.Services.AddTransient<IArticleService, ArticleService>();
            builder.Services.AddTransient<ISourceService, SourceService>();
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id:int?}/");

            app.MapControllerRoute(
                name: "name",
                pattern: "api/{controller}/{action}/{name}/{age?}",
                constraints: new {age = new IntRouteConstraint()}
                );

            app.Run();
        }
    }
}