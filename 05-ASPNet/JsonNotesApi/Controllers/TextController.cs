using Microsoft.AspNetCore.Mvc;

namespace JsonNotesApi;

[ApiController]
[Route("api/text")]
public class TextController : ControllerBase
{
    [HttpPost("uppercase")]
    public IActionResult Uppercase(TextRequest request)
    {
        return Ok(new { text = request.Text.ToUpperInvariant() });
    }

    [HttpPost("trim")]
    public IActionResult Trim(TextRequest request)
    {
        return Ok(new { text = request.Text.Trim() });
    }
}
