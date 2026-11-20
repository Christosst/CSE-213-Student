using System.ComponentModel.DataAnnotations;

namespace UniversityApiTemplate;

// Input sent by the client when creating or updating a course section.
public class CourseSectionRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "CourseId must be a valid positive ID.")]
    public int CourseId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "TeacherId must be a valid positive ID.")]
    public int? TeacherId { get; set; }

    [Required]
    [StringLength(30)]
    public string SectionCode { get; set; } = "";

    [Required]
    [StringLength(50)]
    public string Term { get; set; } = "";

    [StringLength(50)]
    public string Room { get; set; } = "";
}
