
using BlazorEcommerce.StartupConfig;


var builder = WebApplication.CreateBuilder(args);

builder.AddServices();
builder.AddCustomServices();
builder.AddAuthServices();
builder.AddHttpClientService();

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
