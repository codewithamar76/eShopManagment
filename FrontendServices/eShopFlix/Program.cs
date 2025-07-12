using eShopFlix.HttpClients;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient("HttpClient", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiGatewayAddress"]); // Adjust the URL as needed
});

builder.Services.AddScoped<AuthServiceClient>(provider =>{
    var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
    return new AuthServiceClient(httpClientFactory.CreateClient("HttpClient"));
});

builder.Services.AddScoped<CatalogServiceClient>(provider => {
    var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
    return new CatalogServiceClient(httpClientFactory.CreateClient("HttpClient"));
});

builder.Services.AddScoped<CartServiceClient>(provider => {
    var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
    return new CartServiceClient(httpClientFactory.CreateClient("HttpClient"));
});

builder.Services.AddScoped<PaymentServiceClient>(provider => {
    var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
    return new PaymentServiceClient(httpClientFactory.CreateClient("HttpClient"));
});

builder.Services.AddScoped<OrderServiceClient>(provider => {
    var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
    return new OrderServiceClient(httpClientFactory.CreateClient("HttpClient"));
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "eShopFlixCookie"; // Set a unique cookie name
        options.LoginPath = "/Account/Login"; // Adjust the path to your login action
        options.AccessDeniedPath = "/Account/AccessDenied"; // Adjust the path to your access denied action
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
  name: "areas",
  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
