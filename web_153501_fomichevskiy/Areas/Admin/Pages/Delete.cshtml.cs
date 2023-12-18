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

namespace API.NewFolder
{
    public class DeleteModel : PageModel
    {
        private readonly IProductService _service;
        private readonly ICategoryService _categoryService;

        public DeleteModel(IProductService service, ICategoryService categoryService)
        {
            _service = service;
            _categoryService = categoryService;
        }

        [BindProperty]
      public VideoGame VideoGame { get; set; } = default!;

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

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                await _service.DeleteProductAsync(id.Value);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }

            return RedirectToPage("./Index");
        }
    }
}
