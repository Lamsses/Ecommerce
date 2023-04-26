using AspNetCoreRateLimit;
using EcommerceLibrary.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using EcommerceLibrary.Constants;
using System.Text;
using EcommerceLibrary.Models;

namespace EcommerceWebApi.StartupConfig;

public static class DependnciyInjctionExtention
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddHealthChecks();
    }
    public static void AddCustomServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<ISqlDataAccess, SqlDataAccess>();
        builder.Services.AddSingleton<ICustomersLogData, CustomersLogData>();
        builder.Services.AddSingleton<ICustomerCouponData, CustomerCouponData>();
        builder.Services.AddSingleton<IAdminLog, AdminLogData>();
        builder.Services.AddSingleton<IProductsData, ProductsData>();
        builder.Services.AddSingleton<IProductsData, ProductsData>();
        builder.Services.AddSingleton<IProdcutCategoryData, ProdcutCategoryData>();


        builder.Services.AddSingleton<IOrdersProductsData, OrdersProductsData>();
        builder.Services.AddSingleton<ICategoriesData, CategoriesData>();
        builder.Services.AddSingleton<ICustomersData, CustomersData>();
        builder.Services.AddSingleton<IOrdersData, OrdersData>();
        builder.Services.AddSingleton<ICouponData, CouponData>();
        builder.Services.AddSingleton<IAnalyticsData, AnalyticsData>();
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    }
    public static void AddAuthServices(this WebApplicationBuilder builder)
    {

        builder.Services.AddAuthorization(opts =>
        {
            opts.AddPolicy("Admin", policy =>
            {
                policy.RequireClaim("role_id","2","1");

            });
            opts.AddPolicy("SuperAdmin", policy =>
            {
                policy.RequireClaim("role_id", "1");

            });
            opts.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });

        builder.Services.AddAuthentication("Bearer")
                   .AddJwtBearer(opts =>
                   {
                       opts.TokenValidationParameters = new()
                       {
                           ValidateIssuer = true,
                           ValidateAudience = true,
                           ValidateIssuerSigningKey = true,
                           ValidIssuer = builder.Configuration.GetValue<string>("Authentication:Issuer"),
                           ValidAudience = builder.Configuration.GetValue<string>("Authentication:Audience"),
                           IssuerSigningKey = new SymmetricSecurityKey(
                               Encoding.ASCII.GetBytes(
                               builder.Configuration.GetValue<string>("Authentication:SecretKey")))
                       };
                   });

    }
    public static void AddRateLimitServices(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<IpRateLimitOptions>(
        builder.Configuration.GetSection("IpRateLimiting"));
        builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
        builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
        builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        builder.Services.AddInMemoryRateLimiting();
    }
}
