using System.ComponentModel.DataAnnotations;

namespace UniversityApiTemplate;

// Defines the data the client sends when creating or updating a student.
// The attributes automatically validate user input.
public class StudentRequest
{
    // Full name is required and cannot be longer than 100 characters
    [Required]
    [StringLength(100)]
    public string FullName { get; set; } = "";

    // Email is required and must follow standard email formatting
    [Required]
    [EmailAddress]
    [StringLength(120)]
    public string Email { get; set; } = "";

    // Major is required, up to 80 characters
    [Required]
    [StringLength(80)]
    public string Major { get; set; } = "";
}
