using Microsoft.AspNetCore.Mvc;
using MovieTracker.API.Dtos;
using MovieTracker.API.Models;
using MovieTracker.API.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaItemsController : ControllerBase
    {
        // Inject IMovieService into the controller
        private readonly IMediaItemsService _service;

        // Constructor that accepts IMediaItemsService and initializes the private field
        public MediaItemsController(IMediaItemsService service)
        {
            _service = service; // Store it in the private field
        }

        // GET: api/MediaItems
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var mediaItems = await _service.GetAllAsync(); // Fetch all media items from the service
            return Ok(mediaItems); // Return the list of media items
        }

        // GET: api/MediaItems/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var mediaItem = await _service.GetByIdAsync(id); // Fetch a specific media item by ID
            if (mediaItem == null)
                return NotFound(); // If the media item is not found, return 404 Not Found

            return Ok(mediaItem); // If found, return the media item
        }

        // POST: api/MediaItems
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] MediaItemRequestDto dto)
        {
            if (dto == null)
                return BadRequest("Data is required.");

            if (string.IsNullOrWhiteSpace(dto.Type))
                return BadRequest("Type is required.");

            if (string.IsNullOrWhiteSpace(dto.Title))
                return BadRequest("Title is required.");

            if (string.IsNullOrWhiteSpace(dto.Genre))
                return BadRequest("Genre is required.");

            if (string.IsNullOrWhiteSpace(dto.Status))
                return BadRequest("Status is required.");

            if (dto.Date == default)
                return BadRequest("Date is required.");

            if (dto.Status == "watched")
            {
                if (dto.Rating == null || dto.Rating < 1 || dto.Rating > 10)
                    return BadRequest("Rating must be 1–10 when status is watched.");
            }

            var model = new MediaItem
            {
                Type = dto.Type,
                Title = dto.Title,
                Genre = dto.Genre,
                Status = dto.Status,
                Rating = dto.Rating,
                Date = dto.Date,          
                Notes = dto.Notes
            };

            var added = await _service.AddAsync(model);

            return CreatedAtAction(nameof(GetById),
                new { id = added.Id },
                ToResponseDto(added));
        }

        // PUT: api/MediaItems/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] MediaItem mediaItem)
        {
            if (mediaItem == null)
                return BadRequest("Invalid data.");

            // Call the service to update the media item
            var updatedMediaItem = await _service.UpdateAsync(mediaItem);

            // Check if the item is null (not found)
            if (updatedMediaItem == null)
                return NotFound();  // If item not found, return 404

            return Ok(updatedMediaItem);  // Return the updated item with 200 OK
        }


        // DELETE: api/MediaItems/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            // Call the service to delete the media item by its ID
            await _service.DeleteAsync(id);

            // Return a 204 No Content response to indicate successful deletion
            return NoContent();
        }

        // Convert MediaItem to ResponseDto
        private MediaItemResponseDto ToResponseDto(MediaItem mediaItem)
        {
            return new MediaItemResponseDto
            {
                Id = mediaItem.Id,
                Type = mediaItem.Type,
                Title = mediaItem.Title,
                Genre = mediaItem.Genre,
                Status = mediaItem.Status,
                Rating = mediaItem.Rating,
                Date = mediaItem.Date,
                Notes = mediaItem.Notes
            };
        }
    }
}
