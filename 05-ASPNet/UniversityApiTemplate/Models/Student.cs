namespace UniversityApiTemplate;

// Represents a student record saved on the server.
public class Student
{
    // The server assigns this unique ID
    public int Id { get; set; }

    // Core student information
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Major { get; set; } = "";

    // Extended academic fields from university database
    public string? RegistrationNumber { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Gender { get; set; }
    public string? School { get; set; }
    public string? Program { get; set; }
    public string? Department { get; set; }
}
