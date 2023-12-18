using Domain.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using web_153501_fomichevskiy.Services.ProductService;

namespace web_153501_fomichevskiy.Areas.Admin.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IProductService _service;

        public IndexModel(IProductService service)
        {
			_service = service;
        }

        public IList<VideoGame> VideoGame { get;set; } = default!;

        public async Task OnGetAsync()
        {
			var response = await _service.GetProductListAsync(null);

			if (response.Success)
			{
				VideoGame = response.Data?.Items!;
			}
		}
    }
}
