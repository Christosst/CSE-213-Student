namespace UniversityApiTemplate;

// Represents an enrollment record linking a student to a course.
public class Enrollment
{
    // The server assigns this unique ID
    public int Id { get; set; }

    // Relationships
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public int? CourseSectionId { get; set; }

    // When the student enrolled
    public DateOnly EnrolledOn { get; set; }
}
