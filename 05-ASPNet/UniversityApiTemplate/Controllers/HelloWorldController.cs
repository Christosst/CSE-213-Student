using Microsoft.AspNetCore.Mvc;

namespace UniversityApiTemplate.Controllers;

// [ApiController] marks this class as a Web API controller.
// [Route("api/hello")] sets the base endpoint URL: http://localhost:5086/api/hello
[ApiController]
[Route("api/hello")]
public class HelloWorldController : ControllerBase
{
    // GET /api/hello
    // A simple test endpoint to confirm your API is running and responding
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            message = "Hello, World! Your University API template is running.",
            time = DateTime.UtcNow
        });
    }

    // GET /api/hello/{name}
    // Demonstrates route parameters (e.g. http://localhost:5086/api/hello/Maria)
    [HttpGet("{name}")]
    public IActionResult Greet(string name)
    {
        return Ok(new
        {
            message = $"Hello, {name}! Welcome to Web Programming."
        });
    }
}
