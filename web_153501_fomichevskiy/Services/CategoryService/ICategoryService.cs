using Domain.Entities;
using Domain.Models;

namespace web_153501_fomichevskiy.Services.CategoryService
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
