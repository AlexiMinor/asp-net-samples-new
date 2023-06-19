using System.Reflection;
using System.Text;
using AspNetSamples.Abstractions.Data.Repositories;
using AspNetSamples.Abstractions.Services;
using AspNetSamples.Abstractions;
using AspNetSamples.Business;
using AspNetSamples.Core.DTOs;
using AspNetSamples.Data.Entities;
using AspNetSamples.Data;
using AspNetSamples.Data.CQS.QueriesHandlers;
using AspNetSamples.Repositories;
using Hangfire;
using Hangfire.Logging.LogProviders;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace AspNetSamples.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.


            builder.Services.AddDbContext<NewsAggregatorContext>(
                opt =>
                {
                    var connString = builder.Configuration
                        .GetConnectionString("DefaultConnection");
                    opt.UseSqlServer(connString);
                });

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecurityKey"])),
                    };
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
            builder.Services.AddTransient<IJwtService, JwtService>();
            builder.Services.AddTransient<ITokenService, TokenService>();

            builder.Services.AddMediatR(
                cfg =>
                    cfg.RegisterServicesFromAssemblyContaining<GetUserByRefreshTokenQueryHandler>());

            builder.Services.AddAutoMapper(typeof(Program));

            builder.Services.AddHangfire(config=>
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddHangfireServer();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(opt =>
            {
                opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
                    $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseHangfireDashboard();


            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}