using AspNetSamples.Abstractions;
using AspNetSamples.Abstractions.Data.Repositories;
using AspNetSamples.Abstractions.Services;
using AspNetSamples.Business;
using AspNetSamples.Data;
using AspNetSamples.Data.Entities;
using AspNetSamples.Mvc.Auth;
using AspNetSamples.Mvc.Filters;
using AspNetSamples.Mvc.Middlewares;
using AspNetSamples.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;

namespace AspNetSamples.Mvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //builder.Configuration.AddInMemoryCollection(new Dictionary<string, string>()
            //{
            //    {"User1","Bob"},
            //    {"User2","Alice"},
            //});

            builder.Services.AddDbContext<NewsAggregatorContext>(
                opt =>
            {
                var connString = builder.Configuration
                    .GetConnectionString("DefaultConnection");
                opt.UseSqlServer(connString);
            });

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
            builder.Services.AddScoped<IRepository<Comment>, Repository<Comment>>();
            builder.Services.AddScoped<IRepository<Role>, RoleRepository>();
            builder.Services.AddScoped<IRepository<Source>, Repository<Source>>();
            builder.Services.AddScoped<IRepository<User>, UserRepository>();

            builder.Services.AddTransient<IArticleService, ArticleService>();
            builder.Services.AddTransient<ICommentService, CommentService>();
            builder.Services.AddTransient<IRoleService, RoleService>();
            builder.Services.AddTransient<ISourceService, SourceService>();
            builder.Services.AddTransient<IUserService, UserService>();

            builder.Services.AddScoped<MyCustomResourceFilter>();

            builder.Services.AddAutoMapper(typeof(Program));

            builder.Services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new PathString("/Account/Login");
                    options.AccessDeniedPath = new PathString("/Account/Login");
                });
            builder.Services.AddAuthorization(options =>
                options.AddPolicy("18+Content",
                    policyBuilder
                        => policyBuilder.AddRequirements(new MinAgeRequirement(18))));

            builder.Services.AddControllersWithViews(opt =>
            {
                //opt.Filters.Add<MyCustomActionFilter>();
                //opt.Filters.Add(typeof(MyCustomActionFilter));
                //opt.Filters.Add(new MyCustomActionFilter());
                //opt.Filters.Add(new AuthorizeFilter());
                //opt.Filters.Add(new AllowAnonymousFilter());

            });


            var app = builder.Build();

            //app.UseTestMiddleware();
            //app.http
            //app.UseCors();
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            //app.UseMvc();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
          
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id:int?}/");

            app.MapControllerRoute(
                name: "name",
                pattern: "api/{controller}/{action}/{name}/{age?}",
                constraints: new { age = new IntRouteConstraint() }
                );

            //app.Run(async (context)=>await context.Response.WriteAsync("Hello world"));
            app.Run();
        }
    }
}