using Domain.Models;
using Domain.Entities;
using web_153501_fomichevskiy.Services.CategoryService;

namespace web_153501_fomichevskiy.Services.CategoryService
{
	public class MemoryCategoryService : ICategoryService
	{
		public Task<ResponseData<List<VideoGameCategory>>> GetCategoryListAsync()
		{
			var categories = new List<VideoGameCategory>
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
			var result = new ResponseData<List<VideoGameCategory>>();
			result.Data = categories;
			return Task.FromResult(result);
		}
	}
}
