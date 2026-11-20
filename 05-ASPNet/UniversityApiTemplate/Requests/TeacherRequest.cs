using System.ComponentModel.DataAnnotations;

namespace UniversityApiTemplate;

// Input sent by the client when creating or updating a teacher.
public class TeacherRequest
{
    [Required]
    [StringLength(100)]
    public string FullName { get; set; } = "";

    [Required]
    [EmailAddress]
    [StringLength(120)]
    public string Email { get; set; } = "";

    [Required]
    [StringLength(80)]
    public string Department { get; set; } = "";
}
