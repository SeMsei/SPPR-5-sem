    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using Domain.Entities;
using Domain.Models;
using API.Services;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoGamesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IProductService _service;

        public VideoGamesController(IProductService service)
        {
            //_context = context;
            _service = service;
        }

        // GET: api/VideoGames
        [HttpGet("")]
        [Route("{category}")]
        [Route("page{pageNo}")]
        [Route("{category}/page{pageNo}")]
		[Authorize]
		public async Task<ActionResult<ResponseData<List<VideoGame>>>> GetVideoGame(string? category, int pageNo = 1, int pageSize = 3)
        {
            return Ok(await _service.GetProductListAsync(category, pageNo, pageSize));
        }

        // GET: api/VideoGames/5
        [HttpGet("{id:int}")]
		[Authorize]
		public async Task<ActionResult<VideoGame>> GetVideoGame(int id)
        {
            return Ok(await _service.GetProductByIdAsync(id));
        }

        // PUT: api/VideoGames/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("add_role/{id}")]
        [Authorize]
        public async Task<IActionResult> PutVideoGame(int id, VideoGame videoGame)
        {
            try
            {
                await _service.UpdateProductAsync(id, videoGame);
            }
            catch (Exception e)
            {
                return NotFound(new ResponseData<VideoGame>()
                {
                    Data = null,
                    Success = false,
                    ErrorMessage = e.Message
                });
            }

            return Ok(new ResponseData<VideoGame>()
            {
                Data = videoGame,
            });
        }

        // POST: api/VideoGames
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<VideoGame>> PostVideoGame(VideoGame videoGame)
        {
            if (videoGame == null)
            {
                return BadRequest(new ResponseData<VideoGame>()
                {
                    Data = null,
                    Success = false,
                    ErrorMessage = "Clothes is null"
                });
            }
            var response = await _service.CreateProductAsync(videoGame);

            if (!response.Success)
            {
                return BadRequest(response.ErrorMessage);
            }

            var ret = CreatedAtAction("GetVideoGame", new { id = videoGame.Id }, new ResponseData<VideoGame>()
            {
                Data = videoGame
            });

            return ret;
        }

        // POST: api/Dishes/5
        [HttpPost("{id}")]
        [Authorize]
        public async Task<ActionResult<ResponseData<string>>> PostImage(int id, IFormFile formFile)
        {
            var response = await _service.SaveImageAsync(id, formFile);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        // DELETE: api/VideoGames/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteVideoGame(int id)
        {
            try
            {
                await _service.DeleteProductAsync(id);
            }
            catch (Exception e)
            {
                return NotFound(new ResponseData<VideoGame>()
                {
                    Data = null,
                    Success = false,
                    ErrorMessage = e.Message
                });
            }

            return NoContent();
        }

        private bool VideoGameExists(int id)
        {
            return (_context.VideoGame?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
