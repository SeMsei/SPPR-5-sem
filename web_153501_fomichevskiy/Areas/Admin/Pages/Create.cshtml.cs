using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using API.Data;
using Domain.Entities;
using web_153501_fomichevskiy.Services.ProductService;
using web_153501_fomichevskiy.Services.CategoryService;

namespace API.NewFolder
{
    public class CreateModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public CreateModel(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> OnGet()
        {
            var response = await _categoryService.GetCategoryListAsync();
            if (!response.Success)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(response.Data, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public VideoGame VideoGame { get; set; } = default!;

        [BindProperty]
        public IFormFile? Image { get; set; }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var response = await _productService.CreateProductAsync(VideoGame, Image);

            if (!response.Success)
            {
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
