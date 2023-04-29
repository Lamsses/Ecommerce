
using AspNetCoreRateLimit;
using EcommerceLibrary.DataAccess;
using EcommerceWebApi.StartupConfig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.AddAuthServices();
builder.AddServices();
builder.AddRateLimitServices();
builder.AddCustomServices();

builder.Services.AddMemoryCache();
builder.AddRateLimitServices();




var app = builder.Build();

//app.Urls.Add("http://192.248.185.203:5041");

//app.Urls.Add("http://localhost:5041");
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseHealthChecks("/health");
app.UseAuthorization();

app.MapControllers();
app.UseIpRateLimiting();

app.Run();
