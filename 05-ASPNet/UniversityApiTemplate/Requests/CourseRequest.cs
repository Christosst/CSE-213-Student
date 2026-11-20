using System.ComponentModel.DataAnnotations;

namespace UniversityApiTemplate;

// Input sent by the client when creating or updating a course.
public class CourseRequest
{
    [Required]
    [StringLength(20)]
    public string Code { get; set; } = "";

    [Required]
    [StringLength(150)]
    public string Title { get; set; } = "";

    [Range(1, int.MaxValue)]
    public int? TeacherId { get; set; }

    [Range(1, 30)]
    public int Credits { get; set; } = 6;
}
