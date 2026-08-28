using ApiRoutingDrills.DTOs;
using ApiRoutingDrills.Models;
using Microsoft.AspNetCore.Mvc;
using rest_routing_drills.DTOs;

namespace ApiRoutingDrills.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotesController : ControllerBase
    {
        private static readonly List<Note> _notes = new List<Note>();
        private static int _nextId = 1;

        [HttpPost]
        public IActionResult CreateNote([FromBody] CreateNoteRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Title))
            {
                return BadRequest(new { message = "Title is required" });
            }

            var note = new Note
            {
                Id = _nextId++,
                Title = request.Title,
                Content = request.Content,
                CreatedAt = DateTime.UtcNow
            };

            _notes.Add(note);

            return CreatedAtAction(nameof(GetNoteById), new { id = note.Id }, note);
        }

        [HttpGet]
        public IActionResult GetAllNotes()
        {
            return Ok(_notes);
        }

        [HttpGet("search")]
        public IActionResult SearchNotes([FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return BadRequest(new { message = "Keyword is required" });
            }

            var results = _notes.Where(n =>
                n.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                n.Content.Contains(keyword, StringComparison.OrdinalIgnoreCase)
            ).ToList();

            return Ok(results);
        }

        [HttpGet("{id}")]
        public IActionResult GetNoteById(int id)
        {
            var note = _notes.FirstOrDefault(n => n.Id == id);
            if (note == null)
            {
                return NotFound(new { message = "Note not found" });
            }

            return Ok(note);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateNote(int id, [FromBody] UpdateNoteRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Title))
            {
                return BadRequest(new { message = "Title is required" });
            }

            var note = _notes.FirstOrDefault(n => n.Id == id);
            if (note == null)
            {
                return NotFound(new { message = "Note not found" });
            }

            note.Title = request.Title;
            note.Content = request.Content;

            return Ok(note);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteNote(int id)
        {
            var note = _notes.FirstOrDefault(n => n.Id == id);
            if (note == null)
            {
                return NotFound(new { message = "Note not found" });
            }

            _notes.Remove(note);
            return NoContent(); // HTTP 204
        }
        /*

        // Drill 12: GET /api/notes?pageNumber=1&pageSize=5 (Pagination Demo)
        [HttpGet]
        public IActionResult GetAllNotes([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5)
        {
            // Validation
            if (pageNumber <= 0)
            {
                return BadRequest(new { message = "pageNumber must be greater than 0" }); // 400 Bad Request
            }

            if (pageSize < 1 || pageSize > 50)
            {
                return BadRequest(new { message = "pageSize must be between 1 and 50" }); // 400 Bad Request
            }

            var totalCount = _notes.Count;

            // Skip / Take Logic
            var items = _notes
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(new
            {
                items,
                pageNumber,
                pageSize,
                totalCount
            });
        }
            */
        
    }
}