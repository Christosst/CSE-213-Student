using System.Text.Json;

namespace UniversityApiTemplate;

// Persistent storage for courses using a local JSON file (courses.json).
public class JsonCourseStore
{
    private readonly string filePath;

    private readonly JsonSerializerOptions jsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

    public JsonCourseStore(IWebHostEnvironment env)
    {
        filePath = Path.Combine(env.ContentRootPath, "App_Data", "courses.json");
    }

    private List<Course> ReadCoursesFromFile()
    {
        if (!File.Exists(filePath))
        {
            return new List<Course>();
        }

        string jsonText = File.ReadAllText(filePath);
        var courses = JsonSerializer.Deserialize<List<Course>>(jsonText, jsonOptions);

        return courses ?? new List<Course>();
    }

    private void WriteCoursesToFile(List<Course> courses)
    {
        string jsonText = JsonSerializer.Serialize(courses, jsonOptions);
        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!); File.WriteAllText(filePath, jsonText);
    }

    public List<Course> GetAll()
    {
        return ReadCoursesFromFile();
    }

    public Course? GetById(int id)
    {
        List<Course> courses = ReadCoursesFromFile();
        foreach (var course in courses)
        {
            if (course.Id == id)
            {
                return course;
            }
        }
        return null;
    }

    public Course Add(CourseRequest request)
    {
        List<Course> courses = ReadCoursesFromFile();

        int nextId = 1;
        foreach (var course in courses)
        {
            if (course.Id >= nextId)
            {
                nextId = course.Id + 1;
            }
        }

        var newCourse = new Course
        {
            Id = nextId,
            Code = request.Code.Trim().ToUpperInvariant(),
            Title = request.Title.Trim(),
            TeacherId = request.TeacherId,
            Credits = request.Credits
        };

        courses.Add(newCourse);
        WriteCoursesToFile(courses);

        return newCourse;
    }

    public bool Update(int id, CourseRequest request)
    {
        List<Course> courses = ReadCoursesFromFile();

        Course? courseToUpdate = null;
        foreach (var course in courses)
        {
            if (course.Id == id)
            {
                courseToUpdate = course;
                break;
            }
        }

        if (courseToUpdate == null)
        {
            return false;
        }

        courseToUpdate.Code = request.Code.Trim().ToUpperInvariant();
        courseToUpdate.Title = request.Title.Trim();
        courseToUpdate.TeacherId = request.TeacherId;
        courseToUpdate.Credits = request.Credits;

        WriteCoursesToFile(courses);
        return true;
    }

    public bool Delete(int id)
    {
        List<Course> courses = ReadCoursesFromFile();

        Course? courseToDelete = null;
        foreach (var course in courses)
        {
            if (course.Id == id)
            {
                courseToDelete = course;
                break;
            }
        }

        if (courseToDelete == null)
        {
            return false;
        }

        courses.Remove(courseToDelete);
        WriteCoursesToFile(courses);
        return true;
    }

    // Helper: checks if any course is assigned to a specific teacher
    public bool AnyWithTeacher(int teacherId)
    {
        List<Course> courses = ReadCoursesFromFile();
        foreach (var course in courses)
        {
            if (course.TeacherId == teacherId)
            {
                return true;
            }
        }
        return false;
    }
}
