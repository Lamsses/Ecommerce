using EcommerceLibrary.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
builder.Services.AddSingleton<ISqlDataAccess, SqlDataAccess>();
builder.Services.AddSingleton<IProductsData, ProductsData>();
builder.Services.AddSingleton<ICategoriesData, CategoriesData>();
builder.Services.AddSingleton<IOrdersData, OrdersData>();



builder.Services.AddAuthorization(opts =>
{
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
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseHealthChecks("/health");
app.UseAuthorization();

app.MapControllers();

app.Run();
