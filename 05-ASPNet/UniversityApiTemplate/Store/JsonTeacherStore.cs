using System.Text.Json;

namespace UniversityApiTemplate;

// Persistent storage for teachers using a local JSON file (teachers.json).
public class JsonTeacherStore
{
    private readonly string filePath;

    private readonly JsonSerializerOptions jsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

    public JsonTeacherStore(IWebHostEnvironment env)
    {
        filePath = Path.Combine(env.ContentRootPath, "App_Data", "teachers.json");
    }

    private List<Teacher> ReadTeachersFromFile()
    {
        if (!File.Exists(filePath))
        {
            return new List<Teacher>();
        }

        string jsonText = File.ReadAllText(filePath);
        var teachers = JsonSerializer.Deserialize<List<Teacher>>(jsonText, jsonOptions);

        return teachers ?? new List<Teacher>();
    }

    private void WriteTeachersToFile(List<Teacher> teachers)
    {
        string jsonText = JsonSerializer.Serialize(teachers, jsonOptions);
        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!); File.WriteAllText(filePath, jsonText);
    }

    public List<Teacher> GetAll()
    {
        return ReadTeachersFromFile();
    }

    public Teacher? GetById(int id)
    {
        List<Teacher> teachers = ReadTeachersFromFile();
        foreach (var teacher in teachers)
        {
            if (teacher.Id == id)
            {
                return teacher;
            }
        }
        return null;
    }

    public Teacher Add(TeacherRequest request)
    {
        List<Teacher> teachers = ReadTeachersFromFile();

        int nextId = 1;
        foreach (var teacher in teachers)
        {
            if (teacher.Id >= nextId)
            {
                nextId = teacher.Id + 1;
            }
        }

        var newTeacher = new Teacher
        {
            Id = nextId,
            FullName = request.FullName.Trim(),
            Email = request.Email.Trim(),
            Department = request.Department.Trim()
        };

        teachers.Add(newTeacher);
        WriteTeachersToFile(teachers);

        return newTeacher;
    }

    public bool Update(int id, TeacherRequest request)
    {
        List<Teacher> teachers = ReadTeachersFromFile();

        Teacher? teacherToUpdate = null;
        foreach (var teacher in teachers)
        {
            if (teacher.Id == id)
            {
                teacherToUpdate = teacher;
                break;
            }
        }

        if (teacherToUpdate == null)
        {
            return false;
        }

        teacherToUpdate.FullName = request.FullName.Trim();
        teacherToUpdate.Email = request.Email.Trim();
        teacherToUpdate.Department = request.Department.Trim();

        WriteTeachersToFile(teachers);
        return true;
    }

    public bool Delete(int id)
    {
        List<Teacher> teachers = ReadTeachersFromFile();

        Teacher? teacherToDelete = null;
        foreach (var teacher in teachers)
        {
            if (teacher.Id == id)
            {
                teacherToDelete = teacher;
                break;
            }
        }

        if (teacherToDelete == null)
        {
            return false;
        }

        teachers.Remove(teacherToDelete);
        WriteTeachersToFile(teachers);
        return true;
    }

    // Helper: checks if a teacher with this email already exists
    public bool EmailExists(string email, int? excludeId = null)
    {
        List<Teacher> teachers = ReadTeachersFromFile();
        string cleanEmail = email.Trim().ToLowerInvariant();
        foreach (var teacher in teachers)
        {
            if (teacher.Email.Trim().ToLowerInvariant() == cleanEmail)
            {
                if (excludeId.HasValue && teacher.Id == excludeId.Value)
                {
                    continue;
                }
                return true;
            }
        }
        return false;
    }
}
