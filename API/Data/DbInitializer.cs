using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
	public class DbInitializer
	{
		public static async Task SeedData(WebApplication app)
		{
			var _categories = new List<VideoGameCategory>
			{
				new VideoGameCategory {Id=1, Name="RPG",
					NormalizedName="rpg"},
				new VideoGameCategory {Id=2, Name="Survival Horror",
					NormalizedName="survival_horror"},
				new VideoGameCategory {Id=3, Name="RTS",
					NormalizedName="rts"},
				new VideoGameCategory {Id=4, Name="Stealth",
					NormalizedName="stealth"},
				new VideoGameCategory {Id=5, Name="Immersive Sim",
					NormalizedName="immersive_sim"}
			};
			var _games = new List<VideoGame>()
			{
				 new VideoGame() { Name="Arcanum",
					 Description="Компьютерная игра в жанре RPG.",
					 Price =200, Path="Arcanum.jpg",
					 Category = _categories.Find(c=>c.NormalizedName.Equals("rpg"))},
				 new VideoGame() {  Name="Penumbra",
					 Description="Компьютерная игра в жанре survival horror.",
					 Price =330, Path="Penumbra.jpg",
					 Category = _categories.Find(c=>c.NormalizedName.Equals("survival_horror"))},
				 new VideoGame() {  Name="Dota 2",
					 Description="Весёлая игра для всей семьи.",
					 Price =330, Path="Dota.jpg",
					 Category = _categories.Find(c=>c.NormalizedName.Equals("rts"))},
				 new VideoGame() { Name="Siren",
					 Description="Компьютерная игра в жанре survival horror.",
					 Price =330, Path="Siren.jpg",
					 Category = _categories.Find(c=>c.NormalizedName.Equals("survival_horror"))},
				 new VideoGame() {  Name="Thief",
					 Description="Разработана и выпущена в 1998 году.",
					 Price =330, Path="Thief.jpg",
					 Category = _categories.Find(c=>c.NormalizedName.Equals("stealth"))},
				 new VideoGame() {  Name="Weird West",
					 Description="Weird West — компьютерная ролевая игра.",
					 Price =330, Path="WeirdWest.jpg",
					 Category = _categories.Find(c=>c.NormalizedName.Equals("immersive_sim"))},
				 new VideoGame() {  Name="Периметр",
					 Description="Компьютерная игра в жанре стратегии в реальном времени.",
					 Price =330, Path="Perimetr.jpg",
					 Category = _categories.Find(c=>c.NormalizedName.Equals("rts"))},
				 new VideoGame() {  Name="Периметр",
					 Description="Компьютерная игра в жанре стратегии в реальном времени.",
					 Price =330, Path="Perimetr.jpg",
					 Category = _categories.Find(c=>c.NormalizedName.Equals("rts"))},
				 new VideoGame() { Name="Периметр",
					 Description="Компьютерная игра в жанре стратегии в реальном времени.",
					 Price =330, Path="Perimetr.jpg",
					 Category = _categories.Find(c=>c.NormalizedName.Equals("rts"))},
				 new VideoGame() {  Name="Периметр",
					 Description="Компьютерная игра в жанре стратегии в реальном времени.",
					 Price =330, Path="Perimetr.jpg",
					 Category = _categories.Find(c=>c.NormalizedName.Equals("rts"))},
			};

			using var scope = app.Services.CreateScope();
			var context =
			scope.ServiceProvider.GetRequiredService<AppDbContext>();

			if (context.Database.GetPendingMigrations().Any())
			{
				await context.Database.MigrateAsync();
			}

			if (!context.VideoGameCategory.Any())
			{
				await context.VideoGameCategory.AddRangeAsync(_categories!);
				await context.SaveChangesAsync();
			}

			var imageBaseUrl = app.Configuration.GetValue<string>("imageUrl");
			if (!context.VideoGame.Any())
			{

				foreach (var game in _games)
				{
					game.Path = $"{imageBaseUrl}/{game.Path}";
				}
				await context.VideoGame.AddRangeAsync(_games);
				await context.SaveChangesAsync();
			}
		}
	}
}
