using BlazorEcommerce.Models;
using BlazorEcommerce.Services.Interface;
using BlazorEcommerce.Services;
using BlazorEcommerce.Shared;
using EcommerceLibrary.Constants;
using EcommerceLibrary.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using EcommerceLibrary.Models;
using Blazored.LocalStorage;
using Blazored.Toast;

namespace BlazorEcommerce.StartupConfig;

public static class DependnciyInjctionExtention
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();
        builder.Services.AddBlazoredToast();
        builder.Services.AddBlazoredLocalStorage();
        builder.Services.AddAuthorizationCore();
    }
    public static void AddCustomServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<TokenModel>();
        builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<IProductCategoryService, ProductCategoryService>();

        builder.Services.AddScoped<ICartService, CartService>();
        builder.Services.AddScoped<ICategoryService, CategoryService>();
        builder.Services.AddScoped<ICustomerService, CustomerService>();
        builder.Services.AddScoped<IAdminLogService, AdminLogService>();
        builder.Services.AddScoped<IOrderProductsService, OrderProductsService>();
        builder.Services.AddScoped<IOrderService, OrderService>();
        builder.Services.AddScoped<List<ProductsModel>>();


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


            }
            );
            
        });
    }
    public static void AddHttpClientService(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpClient("api", opts =>
        {
            opts.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ApiUrl"));
        });
    }



}
