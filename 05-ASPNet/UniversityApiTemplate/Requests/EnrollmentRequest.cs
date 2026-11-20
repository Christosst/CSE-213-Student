using System.ComponentModel.DataAnnotations;

namespace UniversityApiTemplate;

// Input sent by the client when creating or updating an enrollment.
public class EnrollmentRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "StudentId must be a valid positive ID.")]
    public int StudentId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "CourseId must be a valid positive ID.")]
    public int CourseId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "CourseSectionId must be a valid positive ID.")]
    public int? CourseSectionId { get; set; }

    [Required]
    public DateOnly? EnrolledOn { get; set; }
}
