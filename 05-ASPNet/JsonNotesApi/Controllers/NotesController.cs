using Microsoft.AspNetCore.Mvc;

namespace JsonNotesApi;

// [ApiController] enables automatic 400 Bad Request responses when model validation fails.
// [Route("api/notes")] sets the base URL for this controller: http://localhost:5084/api/notes
[ApiController]
[Route("api/notes")]
public class NotesController : ControllerBase
{
    private readonly JsonNoteStore store;

    // ASP.NET Core automatically supplies the shared JsonNoteStore through Dependency Injection
    public NotesController(JsonNoteStore store)
    {
        this.store = store;
    }

    // GET /api/notes
    // Returns 200 OK with the array of notes read from notes.json
    [HttpGet]
    public IActionResult GetAll()
    {
        var notes = store.GetAll();
        return Ok(notes);
    }

    // GET /api/notes/1
    // Returns 200 OK with the note, or 404 Not Found if the ID does not exist
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var note = store.GetById(id);
        if (note == null)
        {
            return NotFound(); // HTTP 404
        }

        return Ok(note); // HTTP 200 with the note JSON
    }

    // POST /api/notes
    // Reads JSON from the request body into TextRequest.
    // Returns 201 Created with a Location header pointing to GET /api/notes/{id}
    [HttpPost]
    [ProducesResponseType(201)]
    public IActionResult Create(TextRequest request)
    {
        var newNote = store.Add(request);

        // CreatedAtAction creates an HTTP 201 response and adds a 'Location' header
        return CreatedAtAction(nameof(Get), new { id = newNote.Id }, newNote);
    }

    // PUT /api/notes/1
    // Updates the note's text in notes.json.
    // Returns 204 No Content on success, or 404 Not Found if the ID does not exist
    [HttpPut("{id}")]
    public IActionResult Update(int id, TextRequest request)
    {
        bool wasUpdated = store.Update(id, request);
        if (!wasUpdated)
        {
            return NotFound(); // HTTP 404
        }

        return NoContent(); // HTTP 204 (success with an empty response body)
    }

    // DELETE /api/notes/1
    // Deletes the note with the given ID from notes.json.
    // Returns 204 No Content on success, or 404 Not Found if the ID does not exist
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        bool wasDeleted = store.Delete(id);
        if (!wasDeleted)
        {
            return NotFound(); // HTTP 404
        }

        return NoContent(); // HTTP 204
    }
}
