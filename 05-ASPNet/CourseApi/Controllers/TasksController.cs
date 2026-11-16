using Microsoft.AspNetCore.Mvc;

namespace CourseApi;

// [ApiController] enables useful API features, like automatic 400 Bad Request responses when input is invalid.
// [Route("api/tasks")] sets the base URL for this controller: http://localhost:5080/api/tasks
[ApiController]
[Route("api/tasks")]
public class TasksController : ControllerBase
{
    private readonly TaskStore store;

    // ASP.NET Core automatically supplies the shared TaskStore through Dependency Injection
    public TasksController(TaskStore store)
    {
        this.store = store;
    }

    // GET /api/tasks
    // GET /api/tasks?completed=true
    // Returns 200 OK with the array of tasks
    [HttpGet]
    public IActionResult GetAll([FromQuery] bool? completed)
    {
        var taskList = store.GetAll(completed);
        return Ok(taskList);
    }

    // GET /api/tasks/1
    // Returns 200 OK with the task, or 404 Not Found if the ID does not exist
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var task = store.GetById(id);
        if (task == null)
        {
            return NotFound(); // HTTP 404
        }

        return Ok(task); // HTTP 200 with the task in JSON
    }

    // POST /api/tasks
    // Reads JSON from the request body into TaskWriteRequest.
    // Returns 201 Created with a Location header pointing to GET /api/tasks/{id}
    [HttpPost]
    [ProducesResponseType(201)]
    public IActionResult Create(TaskWriteRequest request)
    {
        var newTask = store.Add(request);

        // CreatedAtAction creates an HTTP 201 response and adds a 'Location' header
        return CreatedAtAction(nameof(Get), new { id = newTask.Id }, newTask);
    }

    // PUT /api/tasks/1
    // Replaces the task's title and completed status.
    // Returns 204 No Content on success, or 404 Not Found if the ID does not exist
    [HttpPut("{id}")]
    public IActionResult Update(int id, TaskWriteRequest request)
    {
        bool wasUpdated = store.Update(id, request);
        if (!wasUpdated)
        {
            return NotFound(); // HTTP 404
        }

        return NoContent(); // HTTP 204 (success with an empty response body)
    }

    // DELETE /api/tasks/1
    // Deletes the task with the given ID.
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
