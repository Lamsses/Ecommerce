
using BlazorEcommerce.Models;
using BlazorEcommerce.Shared;
using Blazored.LocalStorage;
using Blazored.SessionStorage;
using EcommerceLibrary.Models;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<TokenModel>();
builder.Services.AddScoped<AuthenticationStateProvider , CustomAuthStateProvider>();
builder.Services.AddScoped<List<ProductsModel>>();
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
