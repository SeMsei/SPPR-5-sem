using API.Data;
using Domain.Entities;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
	public class CategoryService: ICategoryService
	{
        private readonly AppDbContext _dbContext;

        public CategoryService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ResponseData<List<VideoGameCategory>>> GetCategoryListAsync()
        {
            var datalist = new ListModel<VideoGame>();
            var categories = _dbContext.VideoGameCategory.ToListAsync();
            return new ResponseData<List<VideoGameCategory>>()
            {
                Data = await categories
            };
        }
    }
}
