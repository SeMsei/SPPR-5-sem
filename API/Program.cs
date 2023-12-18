using Microsoft.Extensions.Configuration;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.EntityFrameworkCore;
using API.Data;
using static Microsoft.EntityFrameworkCore.DbContextOptionsBuilder;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Services
	.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.Authority = builder
        .Configuration
        .GetSection("isUri").Value;
        opt.TokenValidationParameters.ValidateAudience = false;
        opt.TokenValidationParameters.ValidTypes =
        new[] { "at+jwt" };
    });

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();

// Add services to the container.

builder.Services.AddControllers();
//builder.Services.AddControllersWithViews();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

string connectionString = builder.Configuration.GetConnectionString("Default");
var opt = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(connectionString).Options;
builder.Services.AddScoped((s) => new AppDbContext(opt));
builder.Services.AddDbContext<AppDbContext>();

//string dataDirectory = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar;
//connectionString = string.Format(connectionString!, dataDirectory);

builder.Services.AddCors(options =>
{
	options.AddPolicy("BlazorWasmPolicy", builder =>
	{
		builder.WithOrigins("https://localhost:7191")
			   .AllowAnyMethod()
			   .AllowAnyHeader();
	});
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}


app.UseCors(x => x
			.AllowAnyOrigin()
			.AllowAnyMethod()
			.AllowAnyHeader());

DbInitializer.SeedData(app).Wait();

app.UseHttpsRedirection();

app.UseCors("BlazorWasmPolicy");

app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();

app.MapControllers();

app.Run();
