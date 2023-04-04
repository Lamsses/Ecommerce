using EcommerceLibrary.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSingleton<ISqlDataAccess, SqlDataAccess>();
builder.Services.AddSingleton<ICustomersLogData, CustomersLogData>();

builder.Services.AddSingleton<IAdminLog, AdminLogData>();
builder.Services.AddSingleton<IProductsData, ProductsData>();
builder.Services.AddSingleton<IOrdersProductsData, OrdersProductsData>();
builder.Services.AddSingleton<ICategoriesData, CategoriesData>();
builder.Services.AddSingleton<ICustomersData, CustomersData>();
builder.Services.AddSingleton<IOrdersData, OrdersData>();




builder.Services.AddAuthorization(opts =>
{
    opts.AddPolicy("Admin",policy =>
    {
        policy.RequireClaim("role_id", "2"); 

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
