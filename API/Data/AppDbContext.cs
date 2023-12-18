using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class AppDbContext : DbContext
{
	public DbSet<VideoGame> VideoGame { get; set; }
	public DbSet<VideoGameCategory> VideoGameCategory { get; set; }
	public AppDbContext(DbContextOptions<AppDbContext> options) 
		:base(options)
	{
		//Database.EnsureCreated();
	}
}
