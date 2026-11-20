namespace UniversityApiTemplate;

// Represents a teacher / instructor record saved on the server.
public class Teacher
{
    // The server assigns this unique ID
    public int Id { get; set; }

    // Teacher information
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Department { get; set; } = "";
}
