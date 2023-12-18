using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using web_153501_fomichevskiy.Controllers;
using web_153501_fomichevskiy.Services.ProductService;
using web_153501_fomichevskiy.Services.CategoryService;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Tests;

class CustomComparer : IEqualityComparer<VideoGameCategory>
{
	public bool Equals(VideoGameCategory? x, VideoGameCategory? y)
	{
		if (ReferenceEquals(x, y))
			return true;

		if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
			return false;

		return x.Id == y.Id && x.Name == y.Name && x.NormalizedName == y.NormalizedName;
	}

	public int GetHashCode(VideoGameCategory obj)
	{
		int hash = 0;
		hash = obj.Id.GetHashCode();
		hash = obj.Name.GetHashCode();
		hash = obj.NormalizedName.GetHashCode();
		return hash;
	}
}

public class ControllerTests
{
	private List<VideoGameCategory> TestCategoriesData()
	{
		return new List<VideoGameCategory>() {
				new VideoGameCategory() { Id = 1, Name="RPG", NormalizedName="rpg"},
				new VideoGameCategory() { Id = 2, Name="Stealth", NormalizedName="stealth"}
			};
	}

	private List<VideoGame> TestGamesData()
	{
		return new List<VideoGame>()
				{
					new VideoGame() { Id = 1, Price=123, Name="Game1", CategoryId=1},
					new VideoGame() { Id = 1, Price=234, Name="Game2", CategoryId=2},
				};
	}

	[Fact]
	public void Index_404_categories_unsuccess()
	{
		//Arrange
		Mock<ICategoryService> categories_moq = new();
		categories_moq.Setup(m => m.GetCategoryListAsync()).ReturnsAsync(new ResponseData<List<VideoGameCategory>>()
		{
			Success = false
		});

		Mock<IProductService> clothes_moq = new();
		clothes_moq.Setup(m => m.GetProductListAsync(null, 1)).ReturnsAsync(new ResponseData<ListModel<VideoGame>>()
		{
			Success = true
		});

		var header = new Dictionary<string, StringValues>();
		var controllerContext = new ControllerContext();
		var moqHttpContext = new Mock<HttpContext>();
		moqHttpContext.Setup(c => c.Request.Headers).Returns(new HeaderDictionary(header));
		controllerContext.HttpContext = moqHttpContext.Object;

		//Act
		var controller = new ProductController(clothes_moq.Object, categories_moq.Object) { ControllerContext = controllerContext };
		var result = controller.Index(null).Result;
		//Assert
		Assert.NotNull(result);
		var viewResult = Assert.IsType<NotFoundObjectResult>(result);
		Assert.Equal(StatusCodes.Status404NotFound, viewResult.StatusCode);
	}

	[Fact]
	public void Index_404_products_unsuccess()
	{
		//Arrange
		Mock<ICategoryService> categories_moq = new();
		categories_moq.Setup(m => m.GetCategoryListAsync()).ReturnsAsync(new ResponseData<List<VideoGameCategory>>()
		{
			Success = true
		});

		Mock<IProductService> clothes_moq = new();
		clothes_moq.Setup(m => m.GetProductListAsync(null, 1)).ReturnsAsync(new ResponseData<ListModel<VideoGame>>()
		{
			Success = false
		});

		var header = new Dictionary<string, StringValues>();
		var controllerContext = new ControllerContext();
		var moqHttpContext = new Mock<HttpContext>();
		moqHttpContext.Setup(c => c.Request.Headers).Returns(new HeaderDictionary(header));
		controllerContext.HttpContext = moqHttpContext.Object;

		//Act
		var controller = new ProductController(clothes_moq.Object, categories_moq.Object) { ControllerContext = controllerContext };
		var result = controller.Index(null).Result;
		//Assert
		Assert.NotNull(result);
		var viewResult = Assert.IsType<NotFoundObjectResult>(result);
		Assert.Equal(StatusCodes.Status404NotFound, viewResult.StatusCode);
	}

	[Fact]
	public void Index_not_null_categories_successfull()
	{
		//Arrange
		Mock<ICategoryService> categories_moq = new();
		categories_moq.Setup(m => m.GetCategoryListAsync()).ReturnsAsync(new ResponseData<List<VideoGameCategory>>()
		{
			Data = TestCategoriesData()
		});

		Mock<IProductService> clothes_moq = new();
		clothes_moq.Setup(m => m.GetProductListAsync(null, 1)).ReturnsAsync(new ResponseData<ListModel<VideoGame>>()
		{
			Success = true,
			ErrorMessage = null,
			Data = new ListModel<VideoGame>()
			{
				Items = TestGamesData()
			}
		});

		var header = new Dictionary<string, StringValues>();
		var controllerContext = new ControllerContext();
		var moqHttpContext = new Mock<HttpContext>();
		moqHttpContext.Setup(c => c.Request.Headers).Returns(new HeaderDictionary(header));
		controllerContext.HttpContext = moqHttpContext.Object;


		//Act
		var controller = new ProductController(clothes_moq.Object, categories_moq.Object) { ControllerContext = controllerContext };
		var result = controller.Index(null).Result;
		//Assert
		Assert.NotNull(result);
		var viewResult = Assert.IsType<ViewResult>(result);
		Assert.True(viewResult.ViewData.ContainsKey("categories"));

		Assert.Equal(TestCategoriesData(), viewResult.ViewData["categories"] as IEnumerable<VideoGameCategory>, new CustomComparer());
	}

	[Fact]
	public void Index_valid_category()
	{
		//Arrange
		Mock<ICategoryService> categories_moq = new();
		categories_moq.Setup(m => m.GetCategoryListAsync()).ReturnsAsync(new ResponseData<List<VideoGameCategory>>()
		{
			Data = TestCategoriesData()
		});

		Mock<IProductService> clothes_moq = new();
		clothes_moq.Setup(m => m.GetProductListAsync("rpg", 1)).ReturnsAsync(new ResponseData<ListModel<VideoGame>>()
		{
			Data = new ListModel<VideoGame>()
			{
				Items = TestGamesData()
			}
		});

		var header = new Dictionary<string, StringValues>();
		var controllerContext = new ControllerContext();
		var moqHttpContext = new Mock<HttpContext>();
		moqHttpContext.Setup(c => c.Request.Headers).Returns(new HeaderDictionary(header));
		controllerContext.HttpContext = moqHttpContext.Object;

		//Act
		var controller = new ProductController(clothes_moq.Object, categories_moq.Object) { ControllerContext = controllerContext };
		var result = controller.Index("rpg", 1).Result;
		//Assert
		Assert.NotNull(result);
		var viewResult = Assert.IsType<ViewResult>(result);
		Assert.True(viewResult.ViewData.ContainsKey("currentcategory"));
		Assert.Equal("RPG", viewResult.ViewData["currentcategory"] as string);
	}

	[Fact]
	public void Index_ViewDataContainsValidCurrentCategoryValue_WhenCategoryParameterIsNull()
	{
		//Arrange
		Mock<ICategoryService> categories_moq = new();
		categories_moq.Setup(m => m.GetCategoryListAsync()).ReturnsAsync(new ResponseData<List<VideoGameCategory>>()
		{
			Data = TestCategoriesData()
		});

		Mock<IProductService> clothes_moq = new();
		clothes_moq.Setup(m => m.GetProductListAsync(null, 1)).ReturnsAsync(new ResponseData<ListModel<VideoGame>>()
		{
			Data = new ListModel<VideoGame>()
			{
				Items = TestGamesData()
			}
		});

		var header = new Dictionary<string, StringValues>();
		var controllerContext = new ControllerContext();
		var moqHttpContext = new Mock<HttpContext>();
		moqHttpContext.Setup(c => c.Request.Headers).Returns(new HeaderDictionary(header));
		controllerContext.HttpContext = moqHttpContext.Object;

		//Act
		var controller = new ProductController(clothes_moq.Object, categories_moq.Object) { ControllerContext = controllerContext };
		var result = controller.Index(null).Result;
		//Assert
		Assert.NotNull(result);
		var viewResult = Assert.IsType<ViewResult>(result);
		Assert.True(viewResult.ViewData.ContainsKey("currentcategory"));
		Assert.Equal("Все", viewResult.ViewData["currentcategory"] as string);
	}
}
