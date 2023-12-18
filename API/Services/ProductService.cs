using API.Data;
using Domain.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
	public class ProductService: IProductService
	{
		private readonly int _maxPageSize = 5;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        AppDbContext _context;
		public ProductService(AppDbContext context, [FromServices] IConfiguration configuration,
        IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor) 
		{
			_context = context;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }
		public async Task<ResponseData<ListModel<VideoGame>>> GetProductListAsync(
														string? categoryNormalizedName,
														int pageNo = 1,
														int pageSize = 3)
		{
			if (pageSize > _maxPageSize)
				pageSize = _maxPageSize;
			var query = _context.VideoGame.AsQueryable();
			var dataList = new ListModel<VideoGame>();

			query = query
			.Where(d => categoryNormalizedName == null
			|| d.Category.NormalizedName.Equals(categoryNormalizedName));
			// количество элементов в списке
			var count = query.Count();
			if (count == 0)
			{
				return new ResponseData<ListModel<VideoGame>>
				{
					Data = dataList
				};
			}
			// количество страниц
			int totalPages = (int)Math.Ceiling(count / (double)pageSize);
			if (pageNo > totalPages)
				return new ResponseData<ListModel<VideoGame>>
				{
					Data = null,
					Success = false,
					ErrorMessage = "No such page"
				};
			dataList.Items = await query
			.Skip((pageNo - 1) * pageSize)
			.Take(pageSize)
		   .ToListAsync();
			dataList.CurrentPage = pageNo;
			dataList.TotalPages = totalPages;

			var response = new ResponseData<ListModel<VideoGame>>
			{
				Data = dataList
			};
			return response;
		}

		public async Task<ResponseData<VideoGame>> GetProductByIdAsync(int id)
		{
            var vg = await _context.VideoGame.FindAsync(id);
            if (vg is null)
            {
                return new ResponseData<VideoGame>()
                {
                    Data = null,
                    Success = false,
                    ErrorMessage = "Нет одежды с таким id"
                };
            }

            return new ResponseData<VideoGame>()
            {
                Data = vg
            };
        }
		public async Task UpdateProductAsync(int id, VideoGame product)
		{
            var clothes = await _context.VideoGame.FindAsync(id);
            if (clothes is null)
            {
                throw new ArgumentException("Нет одежды с таким id");
            }

            clothes.Name = product.Name;
            clothes.Description = product.Description;
            clothes.Price = product.Price;
            clothes.CategoryId = product.CategoryId;
            clothes.Category = product.Category;
            _context.Entry(clothes).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
		public async Task DeleteProductAsync(int id)
		{
            var clothes = await _context.VideoGame.FindAsync(id);
            if (clothes is null)
            {
                throw new ArgumentException("Нет одежды с таким id");
            }

            _context.VideoGame.Remove(clothes);
            await _context.SaveChangesAsync();
        }
		public async Task<ResponseData<VideoGame>> CreateProductAsync(VideoGame product)
		{
            _context.VideoGame.Add(product);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return new ResponseData<VideoGame>()
                {
                    Data = null,
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
            return new ResponseData<VideoGame>()
            {
                Data = product
            };
        }
		public async Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
		{
            var responseData = new ResponseData<string>();
            var videoGame = await _context.VideoGame.FindAsync(id);
            if (videoGame == null)
            {
                responseData.Success = false;
                responseData.ErrorMessage = "No item found";
                return responseData;
            }
            var host = "https://" + _httpContextAccessor.HttpContext?.Request.Host;
            var imageFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");

            if (formFile != null)
            {
                if (!string.IsNullOrEmpty(videoGame.Path))
                {
                    var prevImage = Path.GetFileName(videoGame.Path);
                    var prevImagePath = Path.Combine(imageFolder, prevImage);
                    if (File.Exists(prevImagePath))
                    {
                        File.Delete(prevImagePath);
                    }
                }
                var ext = Path.GetExtension(formFile.FileName);
                var fName = Path.ChangeExtension(Path.GetRandomFileName(), ext);
                var filePath = Path.Combine(imageFolder, fName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }

                videoGame.Path = $"{host}/images/{fName}";
                //videoGame.Mime = formFile.ContentType;
                _context.Entry(videoGame).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            responseData.Data = videoGame.Path;
            return responseData;
        }
	}
}
