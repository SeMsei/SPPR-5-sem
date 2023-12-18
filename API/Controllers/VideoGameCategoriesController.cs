using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using Domain.Entities;
using API.Services;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoGameCategoriesController : ControllerBase
    {
        private readonly ICategoryService _service;

        public VideoGameCategoriesController(ICategoryService service)
        {
            _service = service;
        }

        // GET: api/VideoGameCategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VideoGameCategory>>> GetVideoGameCategory()
        {
            var response = await _service.GetCategoryListAsync();
            if (!response.Success)
            {
                return NotFound(response.ErrorMessage);
            }
            return Ok(response);
        }

        // GET: api/VideoGameCategories/5
        /*[HttpGet("{id}")]
        public async Task<ActionResult<VideoGameCategory>> GetVideoGameCategory(int id)
        {
          if (_context.VideoGameCategory == null)
          {
              return NotFound();
          }
            var videoGameCategory = await _context.VideoGameCategory.FindAsync(id);

            if (videoGameCategory == null)
            {
                return NotFound();
            }

            return videoGameCategory;
        }*/

        // PUT: api/VideoGameCategories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /*[HttpPut("{id}")]
        public async Task<IActionResult> PutVideoGameCategory(int id, VideoGameCategory videoGameCategory)
        {
            if (id != videoGameCategory.Id)
            {
                return BadRequest();
            }

            _context.Entry(videoGameCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VideoGameCategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/VideoGameCategories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<VideoGameCategory>> PostVideoGameCategory(VideoGameCategory videoGameCategory)
        {
          if (_context.VideoGameCategory == null)
          {
              return Problem("Entity set 'AppDbContext.VideoGameCategory'  is null.");
          }
            _context.VideoGameCategory.Add(videoGameCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVideoGameCategory", new { id = videoGameCategory.Id }, videoGameCategory);
        }

        // DELETE: api/VideoGameCategories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVideoGameCategory(int id)
        {
            if (_context.VideoGameCategory == null)
            {
                return NotFound();
            }
            var videoGameCategory = await _context.VideoGameCategory.FindAsync(id);
            if (videoGameCategory == null)
            {
                return NotFound();
            }

            _context.VideoGameCategory.Remove(videoGameCategory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VideoGameCategoryExists(int id)
        {
            return (_context.VideoGameCategory?.Any(e => e.Id == id)).GetValueOrDefault();
        }*/
    }
}
