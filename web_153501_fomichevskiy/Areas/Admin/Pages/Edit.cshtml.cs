using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using API.Data;
using Domain.Entities;
using web_153501_fomichevskiy.Services.ProductService;
using web_153501_fomichevskiy.Services.CategoryService;

namespace web_153501_fomichevskiy.Areas.Admin.Pages;

public class EditModel : PageModel
{
    private readonly IProductService _service;
    private readonly ICategoryService _categorySerivce;

    public EditModel(IProductService service, ICategoryService categorySerivce)
    {
        _service = service;
        _categorySerivce = categorySerivce;
    }

    [BindProperty]
    public VideoGame VideoGame { get; set; } = default!;
    [BindProperty]
    public IFormFile? Image { get; set; }

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

        var responseCtg = await _categorySerivce.GetCategoryListAsync();
        if (!response.Success)
        {
            return NotFound();
        }
        ViewData["CategoryId"] = new SelectList(responseCtg.Data, "Id", "Name");

        VideoGame = response.Data!;

        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            await _service.UpdateProductAsync(VideoGame.Id, VideoGame, Image);
        }
        catch (Exception)
        {
            if (!await VideoGameExists(VideoGame.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToPage("./Index");
    }

    private async Task<bool> VideoGameExists(int id)
    {
        var response = await _service.GetProductByIdAsync(id);
        return response.Success;
    }
}
