using System.Text.Json;

namespace UniversityApiTemplate;

// Persistent storage for student enrollments using a local JSON file (enrollments.json).
public class JsonEnrollmentStore
{
    private readonly string filePath;

    private readonly JsonSerializerOptions jsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

    public JsonEnrollmentStore(IWebHostEnvironment env)
    {
        filePath = Path.Combine(env.ContentRootPath, "App_Data", "enrollments.json");
    }

    private List<Enrollment> ReadEnrollmentsFromFile()
    {
        if (!File.Exists(filePath))
        {
            return new List<Enrollment>();
        }

        string jsonText = File.ReadAllText(filePath);
        var enrollments = JsonSerializer.Deserialize<List<Enrollment>>(jsonText, jsonOptions);

        return enrollments ?? new List<Enrollment>();
    }

    private void WriteEnrollmentsToFile(List<Enrollment> enrollments)
    {
        string jsonText = JsonSerializer.Serialize(enrollments, jsonOptions);
        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!); File.WriteAllText(filePath, jsonText);
    }

    public List<Enrollment> GetAll()
    {
        return ReadEnrollmentsFromFile();
    }

    public Enrollment? GetById(int id)
    {
        List<Enrollment> enrollments = ReadEnrollmentsFromFile();
        foreach (var enrollment in enrollments)
        {
            if (enrollment.Id == id)
            {
                return enrollment;
            }
        }
        return null;
    }

    public Enrollment Add(EnrollmentRequest request)
    {
        List<Enrollment> enrollments = ReadEnrollmentsFromFile();

        int nextId = 1;
        foreach (var enrollment in enrollments)
        {
            if (enrollment.Id >= nextId)
            {
                nextId = enrollment.Id + 1;
            }
        }

        var newEnrollment = new Enrollment
        {
            Id = nextId,
            StudentId = request.StudentId,
            CourseId = request.CourseId,
            CourseSectionId = request.CourseSectionId,
            EnrolledOn = request.EnrolledOn!.Value
        };

        enrollments.Add(newEnrollment);
        WriteEnrollmentsToFile(enrollments);

        return newEnrollment;
    }

    public bool Update(int id, EnrollmentRequest request)
    {
        List<Enrollment> enrollments = ReadEnrollmentsFromFile();

        Enrollment? enrollmentToUpdate = null;
        foreach (var enrollment in enrollments)
        {
            if (enrollment.Id == id)
            {
                enrollmentToUpdate = enrollment;
                break;
            }
        }

        if (enrollmentToUpdate == null)
        {
            return false;
        }

        enrollmentToUpdate.StudentId = request.StudentId;
        enrollmentToUpdate.CourseId = request.CourseId;
        enrollmentToUpdate.CourseSectionId = request.CourseSectionId;
        enrollmentToUpdate.EnrolledOn = request.EnrolledOn!.Value;

        WriteEnrollmentsToFile(enrollments);
        return true;
    }

    public bool Delete(int id)
    {
        List<Enrollment> enrollments = ReadEnrollmentsFromFile();

        Enrollment? enrollmentToDelete = null;
        foreach (var enrollment in enrollments)
        {
            if (enrollment.Id == id)
            {
                enrollmentToDelete = enrollment;
                break;
            }
        }

        if (enrollmentToDelete == null)
        {
            return false;
        }

        enrollments.Remove(enrollmentToDelete);
        WriteEnrollmentsToFile(enrollments);
        return true;
    }

    // Helper: checks if student is already enrolled in a specific course
    public bool Exists(int studentId, int courseId, int? excludeId = null)
    {
        List<Enrollment> enrollments = ReadEnrollmentsFromFile();
        foreach (var enrollment in enrollments)
        {
            if (enrollment.StudentId == studentId && enrollment.CourseId == courseId)
            {
                if (excludeId.HasValue && enrollment.Id == excludeId.Value)
                {
                    continue;
                }
                return true;
            }
        }
        return false;
    }

    // Helper: checks if any enrollment is associated with a specific student
    public bool AnyWithStudent(int studentId)
    {
        List<Enrollment> enrollments = ReadEnrollmentsFromFile();
        foreach (var enrollment in enrollments)
        {
            if (enrollment.StudentId == studentId)
            {
                return true;
            }
        }
        return false;
    }

    // Helper: checks if any enrollment is associated with a specific course
    public bool AnyWithCourse(int courseId)
    {
        List<Enrollment> enrollments = ReadEnrollmentsFromFile();
        foreach (var enrollment in enrollments)
        {
            if (enrollment.CourseId == courseId)
            {
                return true;
            }
        }
        return false;
    }

    // Helper: checks if any enrollment is associated with a specific section
    public bool AnyWithSection(int sectionId)
    {
        List<Enrollment> enrollments = ReadEnrollmentsFromFile();
        foreach (var enrollment in enrollments)
        {
            if (enrollment.CourseSectionId == sectionId)
            {
                return true;
            }
        }
        return false;
    }
}
