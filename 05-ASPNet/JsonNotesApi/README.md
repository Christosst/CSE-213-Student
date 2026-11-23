# JSON Notes API: Persistent Storage Without a Database

A complete, beginner-friendly .NET 10 classroom example. Students can create, read, update, and delete text notes stored directly in a local JSON file on the server. A plain HTML/CSS/JavaScript page uses the API, and Swagger UI supports testing. Two extra endpoints process text without saving it.

No database server, SQL, EF Core, or migrations are required. File access and JSON serialization (`System.Text.Json`) are built into .NET.

## Run

From this project's folder:

```powershell
dotnet run --launch-profile classroom
```

From the course demos repository root:

```powershell
dotnet run --project Demos/JsonNotesApi --launch-profile classroom
```

- Student page: <http://localhost:5084/>
- Swagger UI: <http://localhost:5084/swagger/>
- OpenAPI spec: <http://localhost:5084/openapi/v1.json>
- Saved file: `notes.json` directly in this project's folder.

Create a note in the browser, stop the server with `Ctrl+C`, start it again, and reload the page. The note is still there because it was saved to `notes.json`!

## What each file does

| File | Purpose |
|---|---|
| `Program.cs` | Registers services, enables Swagger UI, and maps controllers |
| `Models/Note.cs` | One saved note: `Id` and `Text` |
| `Models/TextRequest.cs` | Permitted input with validation: required text, max 500 characters |
| `JsonNoteStore.cs` | Reads and writes `notes.json` using `File.ReadAllText` and `File.WriteAllText` |
| `Controllers/NotesController.cs` | Full REST CRUD endpoints (`GET`, `POST`, `PUT`, `DELETE`) |
| `Controllers/TextController.cs` | Uppercase and trim operations |
| `wwwroot/index.html` | Form, saved-note list, and browser controls |
| `wwwroot/app.js` | `fetch()` calls, edit/delete actions, and visible errors |
| `wwwroot/site.css` | Basic styling |
| `requests.http` | HTTP requests to test in VS Code or an HTTP client |

## The API contract

| Request | Success | Missing record / bad input |
|---|---|---|
| `GET /api/notes` | 200 and a JSON array `[...]` | Initially returns `[]` |
| `GET /api/notes/{id}` | 200 and one note object | 404 |
| `POST /api/notes` | 201, Location header, and new note | 400 |
| `PUT /api/notes/{id}` | 204 with no body | 404 or 400 |
| `DELETE /api/notes/{id}` | 204 with no body | 404 |
| `POST /api/text/uppercase` | 200 and processed text | 400 |
| `POST /api/text/trim` | 200 and processed text | 400 |

## What is saved on disk?

When you create notes, they are saved directly in `notes.json` in the project root:

```json
[
  {
    "id": 1,
    "text": "Read chapter 28"
  }
]
```

Notice this is a plain JSON array of notes, identical to what JavaScript receives when calling `fetch('/api/notes')`.

## How the store works

Every operation in `JsonNoteStore.cs` follows 3 simple steps:
1. **Load**: Read `notes.json` from disk and deserialize into a C# `List<Note>`.
2. **Modify**: Add, update, or remove the note in the list.
3. **Save**: Serialize the `List<Note>` back to JSON and write it to `notes.json`.

Because `notes.json` is saved on disk, records survive server restarts.

## Suggested reading order for beginners

1. Run the app and create a note on the browser page.
2. Open `notes.json` in VS Code to see your note saved on disk.
3. Read `Models/Note.cs` and `Models/TextRequest.cs`.
4. Read `JsonNoteStore.cs` — notice how `ReadNotesFromFile()` and `WriteNotesToFile()` do all the work.
5. Read `Controllers/NotesController.cs` — notice how it matches `TasksController` in `CourseApi`.
6. Stop and restart the app to prove persistence.
