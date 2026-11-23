using System.ComponentModel.DataAnnotations;

namespace JsonNotesApi;

public class TextRequest
{
    [Required]
    [StringLength(500)]
    public string Text { get; set; } = "";
}
