using System.Text.Json;

namespace UniversityApiTemplate;

// Simple persistent storage using a local JSON file (students.json).
// Saves students to disk so data is kept even if the server is stopped and restarted.
// Beginner-friendly: uses standard loops and no database setup!
public class JsonStudentStore
{
    // The path to the JSON file where students are stored
    private readonly string filePath;

    // Formatting options: WriteIndented = true formats the JSON nicely so students can read it in VS Code
    private readonly JsonSerializerOptions jsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

    public JsonStudentStore(IWebHostEnvironment env)
    {
        // Store students.json in App_Data
        filePath = Path.Combine(env.ContentRootPath, "App_Data", "students.json");
    }

    // Helper: Read students from students.json on disk into a C# List<Student>
    private List<Student> ReadStudentsFromFile()
    {
        // If the file does not exist yet (first run), start with an empty list
        if (!File.Exists(filePath))
        {
            return new List<Student>();
        }

        // Read the text content of students.json
        string jsonText = File.ReadAllText(filePath);

        // Convert the JSON text into a C# List of Student objects
        var students = JsonSerializer.Deserialize<List<Student>>(jsonText, jsonOptions);

        // If the file was empty or null, return a new empty list
        if (students == null)
        {
            return new List<Student>();
        }

        return students;
    }

    // Helper: Write a C# List<Student> back to students.json on disk
    private void WriteStudentsToFile(List<Student> students)
    {
        // Convert the C# List into a formatted JSON string
        string jsonText = JsonSerializer.Serialize(students, jsonOptions);

        // Write the JSON string to students.json
        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!); File.WriteAllText(filePath, jsonText);
    }

    // GET all students
    public List<Student> GetAll()
    {
        return ReadStudentsFromFile();
    }

    // GET one student by ID
    public Student? GetById(int id)
    {
        List<Student> students = ReadStudentsFromFile();

        // Loop through the list to find the student with the matching ID
        foreach (var student in students)
        {
            if (student.Id == id)
            {
                return student; // Found!
            }
        }

        return null; // Not found
    }

    // POST / add a new student
    public Student Add(StudentRequest request)
    {
        List<Student> students = ReadStudentsFromFile();

        // Find the next available unique ID (starts at 1)
        int nextId = 1;
        foreach (var student in students)
        {
            if (student.Id >= nextId)
            {
                nextId = student.Id + 1;
            }
        }

        // Create the new Student object
        var newStudent = new Student
        {
            Id = nextId,
            FullName = request.FullName.Trim(),
            Email = request.Email.Trim(),
            Major = request.Major.Trim()
        };

        // Add the student to our list and save the list to disk
        students.Add(newStudent);
        WriteStudentsToFile(students);

        return newStudent;
    }

    // PUT / update an existing student
    public bool Update(int id, StudentRequest request)
    {
        List<Student> students = ReadStudentsFromFile();

        // Find the student with the matching ID
        Student? studentToUpdate = null;
        foreach (var student in students)
        {
            if (student.Id == id)
            {
                studentToUpdate = student;
                break;
            }
        }

        // If no matching student was found, return false
        if (studentToUpdate == null)
        {
            return false;
        }

        // Update the fields and save to disk
        studentToUpdate.FullName = request.FullName.Trim();
        studentToUpdate.Email = request.Email.Trim();
        studentToUpdate.Major = request.Major.Trim();

        WriteStudentsToFile(students);
        return true; // Successfully updated
    }

    // DELETE a student by ID
    public bool Delete(int id)
    {
        List<Student> students = ReadStudentsFromFile();

        // Find the student with the matching ID
        Student? studentToDelete = null;
        foreach (var student in students)
        {
            if (student.Id == id)
            {
                studentToDelete = student;
                break;
            }
        }

        // If no matching student was found, return false
        if (studentToDelete == null)
        {
            return false;
        }

        // Remove the student from our list and save to disk
        students.Remove(studentToDelete);
        WriteStudentsToFile(students);

        return true; // Successfully deleted
    }

    // Helper: checks if a student with this email already exists
    public bool EmailExists(string email, int? excludeId = null)
    {
        List<Student> students = ReadStudentsFromFile();
        string cleanEmail = email.Trim().ToLowerInvariant();
        foreach (var student in students)
        {
            if (student.Email.Trim().ToLowerInvariant() == cleanEmail)
            {
                if (excludeId.HasValue && student.Id == excludeId.Value)
                {
                    continue;
                }
                return true;
            }
        }
        return false;
    }
}
