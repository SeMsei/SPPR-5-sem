using Microsoft.AspNetCore.Mvc;
using web_153501_fomichevskiy.Extensions;
using web_153501_fomichevskiy.Services.CategoryService;
using web_153501_fomichevskiy.Services.ProductService;
//using System.Runtime.InteropServices.JavaScript.JSType;

namespace web_153501_fomichevskiy.Controllers
{
	[Route("catalog")]
	public class ProductController : Controller
	{
		private IProductService _videoGameService;
		private ICategoryService _videoGameCategoryService;

		public ProductController(IProductService videoGameService, ICategoryService videoGameCategoryService) 
		{
			_videoGameService = videoGameService;
			_videoGameCategoryService = videoGameCategoryService;
		}

		public async Task<IActionResult> Index(string? category, /*string? currentCategory,*/ int pageNo = 1)
		{
			
			ViewData["category"] = category;
			var productResponse = await _videoGameService.GetProductListAsync(category, pageNo);
			if (!productResponse.Success)
			{
				return NotFound(productResponse.ErrorMessage);
			}
			var allCategories = await _videoGameCategoryService.GetCategoryListAsync();

			if (!allCategories.Success)
			{
				return NotFound(productResponse.ErrorMessage);
			}

			ViewData["currentcategory"] = category == null
							 ? "Все"
							 : allCategories
							.Data
							.FirstOrDefault(c => c.NormalizedName == category)?.Name;
			ViewData["categories"] = allCategories.Data;
			ViewData["InAdminArea"] = false;

			//var returnUrl = Request.Path + Request.QueryString.ToUriComponent();
			var req1 = await _videoGameService.GetProductListAsync(category, pageNo);

			if (Request.IsAjaxRequest())
			{
				var returnUrl = Request.Path + Request.QueryString.ToUriComponent();
				return PartialView("_listpart", productResponse.Data);
			}
			return View(productResponse.Data);
		}

	}
}
