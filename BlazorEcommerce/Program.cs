
using BlazorEcommerce.Models;
using BlazorEcommerce.Services;
using BlazorEcommerce.Services.Interface;
using BlazorEcommerce.Shared;
using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Blazored.Toast;
using EcommerceLibrary.Models;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<TokenModel>();
builder.Services.AddScoped<AuthenticationStateProvider , CustomAuthStateProvider>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICustomerService , CustomerService>();
builder.Services.AddScoped<IAdminLogService, AdminLogService>();
builder.Services.AddScoped<IOrderProductsService, OrderProductsService>();
builder.Services.AddScoped<IOrderService, OrderService>();


builder.Services.AddScoped<List<ProductsModel>>();
builder.Services.AddBlazoredToast();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredSessionStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddAuthorization(opts =>
{


    opts.AddPolicy("Admin", policy =>
    {
        policy.RequireClaim("role_id", "2");

    });
    opts.AddPolicy("SuperAdmin", policy =>
    {
        policy.RequireClaim("role_id", "1");

    });
});

builder.Services.AddHttpClient("api" ,opts =>
{
    opts.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ApiUrl"));
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
