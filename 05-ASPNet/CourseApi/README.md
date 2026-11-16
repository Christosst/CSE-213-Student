# Core CourseApi: In-Memory Web API

A beginner-friendly .NET 10 REST API demonstrating controllers, routing, request validation, and in-memory CRUD operations. No database or EF Core is required.

## What each file does

| File | Purpose |
|---|---|
| `Program.cs` | Registers services, enables Swagger UI, and maps controllers |
| `Models/TaskItem.cs` | The shape of a saved task (`Id`, `Title`, `IsCompleted`) |
| `Models/TaskWriteRequest.cs` | Input model with validation rules (`[Required]`, `[StringLength(120)]`) |
| `Models/TextRequest.cs` | Input model for text processing |
| `TaskStore.cs` | In-memory storage using a simple C# `List<TaskItem>` |
| `Controllers/HelloController.cs` | The simplest possible GET endpoint returning a JSON greeting |
| `Controllers/TasksController.cs` | Full REST CRUD endpoints (`GET`, `POST`, `PUT`, `DELETE`) |
| `Controllers/TextController.cs` | Text processing endpoints (`analysis` and `uppercase`) |

## Suggested reading order for beginners

1. `Controllers/HelloController.cs` - see how a method returns a JSON response.
2. `Models/TaskItem.cs` and `Models/TaskWriteRequest.cs` - understand C# classes and validation attributes.
3. `TaskStore.cs` - see how a `List<TaskItem>` is used to add, update, find, and delete items.
4. `Controllers/TasksController.cs` - connect HTTP verbs and status codes (`Ok`, `CreatedAtAction`, `NoContent`, `NotFound`) to store operations.
5. `Program.cs` - see the 4 simple lines needed to configure and start the web server.

## Run instructions

```sh
dotnet run --launch-profile classroom
```

Open:
- Browser frontend: <http://localhost:5080/>
- Swagger UI: <http://localhost:5080/swagger>
