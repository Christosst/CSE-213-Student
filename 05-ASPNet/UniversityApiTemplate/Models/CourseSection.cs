namespace UniversityApiTemplate;

// Represents a specific class section or lab for a course.
public class CourseSection
{
    // The server assigns this unique ID
    public int Id { get; set; }

    // Section details
    public int CourseId { get; set; }
    public int? TeacherId { get; set; }
    public string SectionCode { get; set; } = "";
    public string SectionName { get; set; } = "";
    public string Term { get; set; } = "";
    public string Room { get; set; } = "";

    // Extended section details from university database
    public string? ModeOfStudy { get; set; }
    public int? Capacity { get; set; }
    public string? School { get; set; }
    public string? Program { get; set; }
    public string? Department { get; set; }
    public string? Level { get; set; }
    public decimal? Ects { get; set; }
}
