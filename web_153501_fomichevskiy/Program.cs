using API.Data;
using Microsoft.Extensions.Configuration;
using web_153501_fomichevskiy.Services.CategoryService;
using web_153501_fomichevskiy.Services.ProductService;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbContextOptionsBuilder;
using web_153501_fomichevskiy.Util;
using web_153501_fomichevskiy.TagHelpers;
using web_153501_fomichevskiy.Services.CartService;
using Serilog;
using web_153501_fomichevskiy.Middleware;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<Pager>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();   

UriData uriData = builder.Configuration.GetSection("UriData").Get<UriData>()!;
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddHttpClient<IProductService, ApiProductService>(client =>
{
	client.BaseAddress = new Uri(uriData.ApiUri);
});

builder.Services.AddHttpClient<ICategoryService, ApiCategoryService>(client =>
{
	client.BaseAddress = new Uri(uriData.ApiUri);
});

builder.Services.AddHttpContextAccessor();

//builder.Services.AddScoped<ICategoryService, MemoryCategoryService>();
//builder.Services.AddScoped<IProductService, MemoryProductService>();

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultScheme = "cookie";
    opt.DefaultChallengeScheme = "oidc";
})
 .AddCookie("cookie")
 .AddOpenIdConnect("oidc", options =>
 {
    options.Authority =
    builder.Configuration["InteractiveServiceSettings:AuthorityUrl"];
    options.ClientId =
    builder.Configuration["InteractiveServiceSettings:ClientId"];
    options.ClientSecret =
    builder.Configuration["InteractiveServiceSettings:ClientSecret"];
    options.GetClaimsFromUserInfoEndpoint = true;
    options.ResponseType = "code";
    options.ResponseMode = "query";
    options.SaveTokens = true;
 });

builder.Services.AddScoped(SessionCart.GetCart);

var configuration = new ConfigurationBuilder()
		.SetBasePath(Directory.GetCurrentDirectory())
		.AddJsonFile("appsettings.json")
		.Build();

var logger = new LoggerConfiguration()
	.ReadFrom.Configuration(configuration)
	.CreateLogger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages().RequireAuthorization();
app.UseMiddleware<LoggingMiddleware>(logger);
app.Run();
