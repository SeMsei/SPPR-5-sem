using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using API.Data;
using Domain.Entities;
using web_153501_fomichevskiy.Services.ProductService;
using web_153501_fomichevskiy.Services.CategoryService;

namespace web_153501_fomichevskiy.Areas.Admin.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly IProductService _service;
        private readonly ICategoryService _categoryService;

        public DetailsModel(IProductService service, ICategoryService categoryService)
        {
            _service = service;
            _categoryService = categoryService;
        }

        public VideoGame VideoGame { get; set; } = default!; 
        public VideoGameCategory ctg { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _service.GetProductByIdAsync(id.Value);

            if (!response.Success)
            {
                return NotFound();
            }

            VideoGame = response.Data!;
            ctg = _categoryService.GetCategoryListAsync().Result.Data.ToList().Where(c => c.Id == VideoGame.CategoryId).ToList()[0];

            return Page();
        }
    }
}
