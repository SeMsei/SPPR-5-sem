using IdentityServer;
using Serilog;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using IdentityServer.Data;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");
//app.UseAuthentication();;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((ctx, lc) => lc
        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
        .Enrich.FromLogContext()
        .ReadFrom.Configuration(ctx.Configuration));

	//builder.Services.AddCors(options =>
	//{
	//	options.AddPolicy("BlazorWasmPolicy", builder =>
	//	{
	//		builder.WithOrigins("https://localhost:7191")
	//			   .AllowAnyMethod()
	//			   .AllowAnyHeader();
	//	});
	//});

	builder.Services.AddCors(options =>
	{
		options.AddDefaultPolicy(
			builder =>
			{

				//you can configure your custom policy
				builder.AllowAnyOrigin()
									.AllowAnyHeader()
									.AllowAnyMethod();
			});
	});

	var app = builder
        .ConfigureServices()
        .ConfigurePipeline();	

    // this seeding is only for the template to bootstrap the DB and users.
    // in production you will likely want a different approach.
    //if (args.Contains("/seed"))
    //{
        Log.Information("Seeding database...");
        SeedData.EnsureSeedData(app);
        Log.Information("Done seeding database. Exiting.");
	//return;
	//}
	//app.UseCors("BlazorWasmPolicy");
	//app.UseCors(builder => builder.AllowAnyOrigin());
	//app.UseCors(x => x
	//		.AllowAnyOrigin()
	//		.AllowAnyMethod()
	//		.AllowAnyHeader());
	app.UseCors();
	app.Run();
}
catch (Exception ex) when (
                            // https://github.com/dotnet/runtime/issues/60600
                            ex.GetType().Name is not "StopTheHostException"
                            // HostAbortedException was added in .NET 7, but since we target .NET 6 we
                            // need to do it this way until we target .NET 8
                            && ex.GetType().Name is not "HostAbortedException"
                        )
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}/*var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();*/