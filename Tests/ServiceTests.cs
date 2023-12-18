using API.Data;
using API.Services;
using Domain.Entities;
using Domain.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests;

public class ServiceTests
{
	public void Dispose() => _connection.Dispose();

	private readonly DbConnection _connection;
	private readonly DbContextOptions<AppDbContext> _contextOptions;

	public ServiceTests()
	{
		_connection = new SqliteConnection("Filename=:memory:");
		_connection.Open();

		_contextOptions = new DbContextOptionsBuilder<AppDbContext>()
			.UseSqlite(_connection)
			.Options;

		using var context = new AppDbContext(_contextOptions);
		context.Database.EnsureCreated();
		context.VideoGameCategory.AddRange(
			new VideoGameCategory() { Id = 1, Name = "RPG", NormalizedName = "rpg" },
			new VideoGameCategory() { Id = 2, Name = "RTS", NormalizedName = "rts" });
		context.VideoGame.AddRange(
			new VideoGame() { Id = 1, Price = 100, Name = "Rpg1", CategoryId = 1 },
			new VideoGame() { Id = 2, Price = 101, Name = "Rpg2", CategoryId = 1 },
			new VideoGame() { Id = 3, Price = 102, Name = "Rpg3", CategoryId = 1 },
			new VideoGame() { Id = 4, Price = 103, Name = "Rpg4", CategoryId = 1 },
			new VideoGame() { Id = 5, Price = 104, Name = "Rts1", CategoryId = 2 },
			new VideoGame() { Id = 6, Price = 105, Name = "Rts2", CategoryId = 2 },
			new VideoGame() { Id = 7, Price = 106, Name = "Rts3", CategoryId = 2 });
		context.SaveChanges();
	}

	private AppDbContext CreateContext() => new AppDbContext(_contextOptions);


	[Fact]
	public void ServiceReturnsFirstPageOfThreeItems()
	{
		using var context = CreateContext();
		var service = new ProductService(context, null, null, null);
		var result = service.GetProductListAsync(null).Result;
		Assert.IsType<ResponseData<ListModel<VideoGame>>>(result);
		Assert.True(result.Success);
		Assert.Equal(1, result.Data.CurrentPage);
		Assert.Equal(3, result.Data.Items.Count);
		Assert.Equal(3, result.Data.TotalPages);
		Assert.Equal(context.VideoGame.First(), result.Data.Items[0]);
	}

	[Fact]
	public void ServiceReturnsCorrectPage()
	{
		using var context = CreateContext();
		var service = new ProductService(context, null, null, null);
		var result = service.GetProductListAsync(null, 2).Result;
		Assert.IsType<ResponseData<ListModel<VideoGame>>>(result);
		Assert.True(result.Success);
		Assert.Equal(2, result.Data.CurrentPage);
		Assert.Equal(3, result.Data.Items.Count);
		Assert.Equal(3, result.Data.TotalPages);
		Assert.Equal(context.VideoGame.ToList()[3], result.Data.Items[0]);
	}

	[Fact]
	public void ServiceReturnsCorrectItemCategorySelected()
	{
		using var context = CreateContext();
		var service = new ProductService(context, null, null, null);
		var result = service.GetProductListAsync("rts").Result;
		Assert.IsType<ResponseData<ListModel<VideoGame>>>(result);
		Assert.True(result.Success);
		Assert.Equal(1, result.Data.CurrentPage);
		Assert.Equal(3, result.Data.Items.Count);
		Assert.Equal(1, result.Data.TotalPages);
		Assert.Equal(context.VideoGame.ToList()[4], result.Data.Items[0]);
	}

	[Fact]
	public void ServiceReturnsFalseIncorrectPageNo()
	{
		using var context = CreateContext();
		var service = new ProductService(context, null, null, null);
		var result = service.GetProductListAsync(null, 4).Result;
		Assert.IsType<ResponseData<ListModel<VideoGame>>>(result);
		Assert.False(result.Success);
		Assert.Equal(null, result.Data);
		Assert.Equal(result.ErrorMessage, "No such page");
	}

	[Fact]
	public void ServiceIncorrectMaxPageSize()
	{
		using var context = CreateContext();
		var service = new ProductService(context, null, null, null);
		var result = service.GetProductListAsync(null, 1, 6).Result;
		Assert.IsType<ResponseData<ListModel<VideoGame>>>(result);
		Assert.True(result.Success);
		Assert.Equal(1, result.Data.CurrentPage);
		Assert.Equal(5, result.Data.Items.Count);
		Assert.Equal(2, result.Data.TotalPages);
		Assert.Equal(context.VideoGame.First(), result.Data.Items[0]);
	}
}
