namespace UniversityApiTemplate;

// Represents a course offered by the university.
public class Course
{
    // The server assigns this unique ID
    public int Id { get; set; }

    // Course details
    public string Code { get; set; } = "";
    public string Title { get; set; } = "";
    public int? TeacherId { get; set; }
    public int Credits { get; set; } = 6;

    // Extended academic fields from university database
    public string? School { get; set; }
    public string? Program { get; set; }
    public string? Department { get; set; }
    public string? Level { get; set; }
    public decimal? Ects { get; set; }
}
