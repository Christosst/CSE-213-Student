using Microsoft.AspNetCore.Mvc;

namespace CourseApi;

[ApiController]
[Route("api/text")]
public class TextController : ControllerBase
{
    // POST /api/text/analysis
    [HttpPost("analysis")]
    public IActionResult Analyze(TextRequest request)
    {
        int characters = request.Text.Length;

        // Split text into words using common whitespace characters
        char[] whitespace = [' ', '\t', '\n', '\r'];
        string[] words = request.Text.Split(whitespace, StringSplitOptions.RemoveEmptyEntries);

        return Ok(new { characters, words = words.Length });
    }

    // POST /api/text/uppercase
    [HttpPost("uppercase")]
    public IActionResult Uppercase(TextRequest request)
    {
        return Ok(new { text = request.Text.ToUpperInvariant() });
    }
}

