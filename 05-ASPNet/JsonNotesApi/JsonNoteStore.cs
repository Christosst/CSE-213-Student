using System.Text.Json;

namespace JsonNotesApi;

// Simple persistent storage using a local JSON file.
// Data is saved to a file on disk so it survives server restarts.
// No SQL, database server, or EF Core required!
public class JsonNoteStore
{
    // The path to the JSON file where notes are stored
    private readonly string filePath;

    // Formatting options: WriteIndented = true formats the JSON nicely so students can read it in VS Code
    private readonly JsonSerializerOptions jsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

    public JsonNoteStore(IWebHostEnvironment env)
    {
        // Store notes.json directly in the project's root folder
        filePath = Path.Combine(env.ContentRootPath, "notes.json");
    }

    // Helper: Read notes from the JSON file on disk into a C# List<Note>
    private List<Note> ReadNotesFromFile()
    {
        // If the file does not exist yet (first run), start with an empty list
        if (!File.Exists(filePath))
        {
            return new List<Note>();
        }

        // Read the text content of notes.json
        string jsonText = File.ReadAllText(filePath);

        // Convert the JSON text into a C# List of Note objects
        var notes = JsonSerializer.Deserialize<List<Note>>(jsonText, jsonOptions);

        // If the file was empty or null, return a new empty list
        if (notes == null)
        {
            return new List<Note>();
        }

        return notes;
    }

    // Helper: Write a C# List<Note> back to the JSON file on disk
    private void WriteNotesToFile(List<Note> notes)
    {
        // Convert the C# List into a formatted JSON string
        string jsonText = JsonSerializer.Serialize(notes, jsonOptions);

        // Write the JSON string to notes.json
        File.WriteAllText(filePath, jsonText);
    }

    // GET all notes
    public List<Note> GetAll()
    {
        return ReadNotesFromFile();
    }

    // GET one note by ID
    public Note? GetById(int id)
    {
        List<Note> notes = ReadNotesFromFile();

        // Loop through the notes to find the one with the matching ID
        foreach (var note in notes)
        {
            if (note.Id == id)
            {
                return note; // Found!
            }
        }

        return null; // Not found
    }

    // POST / add a new note
    public Note Add(TextRequest request)
    {
        List<Note> notes = ReadNotesFromFile();

        // Find the next available unique ID (starts at 1)
        int nextId = 1;
        foreach (var note in notes)
        {
            if (note.Id >= nextId)
            {
                nextId = note.Id + 1;
            }
        }

        // Create the new Note object
        var newNote = new Note
        {
            Id = nextId,
            Text = request.Text.Trim()
        };

        // Add the note to our list and save the list to disk
        notes.Add(newNote);
        WriteNotesToFile(notes);

        return newNote;
    }

    // PUT / update an existing note
    public bool Update(int id, TextRequest request)
    {
        List<Note> notes = ReadNotesFromFile();

        // Find the note with the matching ID
        Note? noteToUpdate = null;
        foreach (var note in notes)
        {
            if (note.Id == id)
            {
                noteToUpdate = note;
                break;
            }
        }

        // If no matching note was found, return false
        if (noteToUpdate == null)
        {
            return false;
        }

        // Update the note's text and save to disk
        noteToUpdate.Text = request.Text.Trim();
        WriteNotesToFile(notes);

        return true; // Successfully updated
    }

    // DELETE a note by ID
    public bool Delete(int id)
    {
        List<Note> notes = ReadNotesFromFile();

        // Find the note with the matching ID
        Note? noteToDelete = null;
        foreach (var note in notes)
        {
            if (note.Id == id)
            {
                noteToDelete = note;
                break;
            }
        }

        // If no matching note was found, return false
        if (noteToDelete == null)
        {
            return false;
        }

        // Remove the note from our list and save to disk
        notes.Remove(noteToDelete);
        WriteNotesToFile(notes);

        return true; // Successfully deleted
    }
}
