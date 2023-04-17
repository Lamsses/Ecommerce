
using BlazorEcommerce.StartupConfig;


var builder = WebApplication.CreateBuilder(args);

builder.AddServices();
builder.AddCustomServices();
builder.AddAuthServices();
builder.AddHttpClientService();

var app = builder.Build();
//app.Urls.Add("https://192.248.185.203:7021");


//app.Urls.Add("https://localhost:7021");

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
