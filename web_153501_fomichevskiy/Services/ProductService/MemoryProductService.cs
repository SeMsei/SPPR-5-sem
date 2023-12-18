using web_153501_fomichevskiy.Services.CategoryService;
using Domain.Entities;
using Domain.Models;
using System;
using Microsoft.AspNetCore.Mvc;

namespace web_153501_fomichevskiy.Services.ProductService
{
	public class MemoryProductService : IProductService
	{
		List<VideoGame>? _dishes;
		List<VideoGameCategory>? _categories;
		IConfiguration _config;
		int page_size;
		int page_count;

		public MemoryProductService([FromServices] IConfiguration config, ICategoryService categoryService/*, int pageNo*/)
		{
			_config = config;
			_categories = categoryService.GetCategoryListAsync()?.Result?.Data;
			SetupData();
		}
		/// <summary>
		/// Инициализация списков
		/// </summary>
		private void SetupData()
		{
			_dishes = new List<VideoGame>()
			{
				 new VideoGame() {Id = 1, Name="Arcanum",
					 Description="1234",
					 Price =200, Path="/Images/Arcanum.jpg",
					 CategoryId=
					 _categories.Find(c=>c.NormalizedName.Equals("rpg")).Id,
					 Category = _categories.Find(c=>c.NormalizedName.Equals("rpg"))},
				 new VideoGame() { Id = 2, Name="Penumbra",
					 Description="Компьютерная игра в жанре survival horror.",
					 Price =330, Path="/Images/Penumbra.jpg",
					 CategoryId=
					 _categories.Find(c=>c.NormalizedName.Equals("survival_horror")).Id,
					 Category = _categories.Find(c=>c.NormalizedName.Equals("survival_horror"))},
				 new VideoGame() { Id = 3, Name="Dota 2",
					 Description="Весёлая игра для всей семьи.",
					 Price =330, Path="/Images/Dota.jpg",
					 CategoryId=
					 _categories.Find(c=>c.NormalizedName.Equals("rts")).Id,
					 Category = _categories.Find(c=>c.NormalizedName.Equals("rts"))},
				 new VideoGame() { Id = 4, Name="Siren",
					 Description="Компьютерная игра в жанре survival horror.",
					 Price =330, Path="/Images/Siren.jpg",
					 CategoryId=
					 _categories.Find(c=>c.NormalizedName.Equals("survival_horror")).Id,
					 Category = _categories.Find(c=>c.NormalizedName.Equals("survival_horror"))},
				 new VideoGame() { Id = 5, Name="Thief",
					 Description="Разработана и выпущена в 1998 году.",
					 Price =330, Path="/Images/Thief.jpg",
					 CategoryId=
					 _categories.Find(c=>c.NormalizedName.Equals("stealth")).Id,
					 Category = _categories.Find(c=>c.NormalizedName.Equals("stealth"))},
				 new VideoGame() { Id = 6, Name="Weird West",
					 Description="Weird West — компьютерная ролевая игра.",
					 Price =330, Path="/Images/WeirdWest.jpg",
					 CategoryId=
					 _categories.Find(c=>c.NormalizedName.Equals("immersive_sim")).Id,
					 Category = _categories.Find(c=>c.NormalizedName.Equals("immersive_sim"))},
				 new VideoGame() { Id = 7, Name="Периметр",
					 Description="Компьютерная игра в жанре стратегии в реальном времени.",
					 Price =330, Path="/Images/Perimetr.jpg",
					 CategoryId=
					 _categories.Find(c=>c.NormalizedName.Equals("rts")).Id,
					 Category = _categories.Find(c=>c.NormalizedName.Equals("rts"))},
				 new VideoGame() { Id = 8, Name="Периметр",
					 Description="Компьютерная игра в жанре стратегии в реальном времени.",
					 Price =330, Path="/Images/Perimetr.jpg",
					 CategoryId=
					 _categories.Find(c=>c.NormalizedName.Equals("rts")).Id,
					 Category = _categories.Find(c=>c.NormalizedName.Equals("rts"))},
				 new VideoGame() { Id = 9, Name="Периметр",
					 Description="Компьютерная игра в жанре стратегии в реальном времени.",
					 Price =330, Path="/Images/Perimetr.jpg",
					 CategoryId=
					 _categories.Find(c=>c.NormalizedName.Equals("rts")).Id,
					 Category = _categories.Find(c=>c.NormalizedName.Equals("rts"))},
				 new VideoGame() { Id = 10, Name="Периметр",
					 Description="Компьютерная игра в жанре стратегии в реальном времени.",
					 Price =330, Path="/Images/Perimetr.jpg",
					 CategoryId=
					 _categories.Find(c=>c.NormalizedName.Equals("rts")).Id,
					 Category = _categories.Find(c=>c.NormalizedName.Equals("rts"))},
			};
		}

		public Task<ResponseData<VideoGame>> CreateProductAsync(VideoGame product, IFormFile?
	   formFile) => throw new NotImplementedException();

		public Task DeleteProductAsync(int id) => throw new NotImplementedException();
		public Task UpdateProductAsync(int id, VideoGame product, IFormFile? formFile) => throw new NotImplementedException();
		public Task<ResponseData<VideoGame>> GetProductByIdAsync(int id)=> throw new NotImplementedException();
		public Task<ResponseData<ListModel<VideoGame>>> GetProductListAsync(string?
		categoryNormalizedName, int pageNo = 1)
		{
			page_size = Convert.ToInt32(_config.GetSection("ItemsPerPage").Value);

			var data = _dishes.Where(d => categoryNormalizedName == null || d.Category.NormalizedName.Equals(categoryNormalizedName)).ToList();
			page_count = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(data.Count()) / Convert.ToDouble(page_size)));
			data = data.Skip((pageNo - 1) * page_size).ToList();
			data = data.Take(page_size).ToList();

			return Task.FromResult(new ResponseData<ListModel<VideoGame>>()
			{
				//Success = !(items.Count() == 0),
				Success = true,
				Data = new ListModel<VideoGame>()
				{
					Items = data,
					CurrentPage = pageNo,
					TotalPages = page_count
				},
				ErrorMessage = !(_dishes.Count() == 0) ? "" : "Ошибка"
			});
		}
	}
};
