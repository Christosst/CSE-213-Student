using System.Text.Json;

namespace UniversityApiTemplate;

// Persistent storage for course sections using a local JSON file (sections.json).
public class JsonSectionStore
{
    private readonly string filePath;

    private readonly JsonSerializerOptions jsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

    public JsonSectionStore(IWebHostEnvironment env)
    {
        filePath = Path.Combine(env.ContentRootPath, "App_Data", "sections.json");
    }

    private List<CourseSection> ReadSectionsFromFile()
    {
        if (!File.Exists(filePath))
        {
            return new List<CourseSection>();
        }

        string jsonText = File.ReadAllText(filePath);
        var sections = JsonSerializer.Deserialize<List<CourseSection>>(jsonText, jsonOptions);

        return sections ?? new List<CourseSection>();
    }

    private void WriteSectionsToFile(List<CourseSection> sections)
    {
        string jsonText = JsonSerializer.Serialize(sections, jsonOptions);
        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!); File.WriteAllText(filePath, jsonText);
    }

    public List<CourseSection> GetAll()
    {
        return ReadSectionsFromFile();
    }

    public CourseSection? GetById(int id)
    {
        List<CourseSection> sections = ReadSectionsFromFile();
        foreach (var section in sections)
        {
            if (section.Id == id)
            {
                return section;
            }
        }
        return null;
    }

    public CourseSection Add(CourseSectionRequest request)
    {
        List<CourseSection> sections = ReadSectionsFromFile();

        int nextId = 1;
        foreach (var section in sections)
        {
            if (section.Id >= nextId)
            {
                nextId = section.Id + 1;
            }
        }

        var newSection = new CourseSection
        {
            Id = nextId,
            CourseId = request.CourseId,
            TeacherId = request.TeacherId,
            SectionCode = request.SectionCode.Trim().ToUpperInvariant(),
            Term = request.Term.Trim(),
            Room = request.Room.Trim()
        };

        sections.Add(newSection);
        WriteSectionsToFile(sections);

        return newSection;
    }

    public bool Update(int id, CourseSectionRequest request)
    {
        List<CourseSection> sections = ReadSectionsFromFile();

        CourseSection? sectionToUpdate = null;
        foreach (var section in sections)
        {
            if (section.Id == id)
            {
                sectionToUpdate = section;
                break;
            }
        }

        if (sectionToUpdate == null)
        {
            return false;
        }

        sectionToUpdate.CourseId = request.CourseId;
        sectionToUpdate.TeacherId = request.TeacherId;
        sectionToUpdate.SectionCode = request.SectionCode.Trim().ToUpperInvariant();
        sectionToUpdate.Term = request.Term.Trim();
        sectionToUpdate.Room = request.Room.Trim();

        WriteSectionsToFile(sections);
        return true;
    }

    public bool Delete(int id)
    {
        List<CourseSection> sections = ReadSectionsFromFile();

        CourseSection? sectionToDelete = null;
        foreach (var section in sections)
        {
            if (section.Id == id)
            {
                sectionToDelete = section;
                break;
            }
        }

        if (sectionToDelete == null)
        {
            return false;
        }

        sections.Remove(sectionToDelete);
        WriteSectionsToFile(sections);
        return true;
    }

    // Helper: checks if any section belongs to a course
    public bool AnyWithCourse(int courseId)
    {
        List<CourseSection> sections = ReadSectionsFromFile();
        foreach (var section in sections)
        {
            if (section.CourseId == courseId)
            {
                return true;
            }
        }
        return false;
    }

    // Helper: checks if any section is assigned to a teacher
    public bool AnyWithTeacher(int teacherId)
    {
        List<CourseSection> sections = ReadSectionsFromFile();
        foreach (var section in sections)
        {
            if (section.TeacherId == teacherId)
            {
                return true;
            }
        }
        return false;
    }
}
