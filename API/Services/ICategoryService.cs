using Domain.Entities;
using Domain.Models;

namespace API.Services
{
	public interface ICategoryService
	{
		/// <summary>
		/// Получение списка всех категорий
		/// </summary>
		/// <returns></returns>
		public Task<ResponseData<List<VideoGameCategory>>> GetCategoryListAsync();
	}
}
