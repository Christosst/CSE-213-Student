# UniversityApiTemplate · Student Starter Skeleton
CSE213 Web Programming · European University Cyprus

Welcome to your starter project! All data models, validation schemas, persistent JSON stores, and preloaded university datasets are pre-implemented so you can focus on building your **Controllers** and your **Frontend Web Pages**.

---

## 🚀 Quickstart (How to Run)

1. Open your terminal in this directory:
   ```powershell
   cd "05-ASPNet/UniversityApiTemplate"
   ```
2. Start the application:
   ```powershell
   dotnet run --launch-profile classroom
   ```
3. Open your browser:
   - **Frontend Web Portal**: <http://localhost:5086>
   - **Swagger API Documentation**: <http://localhost:5086/swagger>
4. Click **"Test GET /api/hello"** on the starter page to confirm the API is working!

---

## 🗺️ Code Map (Project Structure)

```text
UniversityApiTemplate/
├── Program.cs                         <- Application entry point & service registration
├── UniversityApiTemplate.csproj       <- Project file (.NET 10)
├── requests.http                      <- Pre-written HTTP requests for REST Client testing
│
├── Controllers/                       <- [YOUR CODE GOES HERE] API Endpoints
│   └── HelloWorldController.cs        <- Working example controller (GET /api/hello)
│
├── Models/                            <- C# Domain Entities (Data structures saved on server)
│   ├── Student.cs                     <- Student entity (Id, FullName, Email, Major, etc.)
│   ├── Teacher.cs                     <- Teacher entity (Id, FullName, Email, Department, etc.)
│   ├── Course.cs                      <- Course entity (Id, Code, Title, Credits, TeacherId)
│   ├── CourseSection.cs               <- Section offering (Id, CourseId, Semester, Room, Capacity)
│   └── Enrollment.cs                  <- Enrollment link (Id, StudentId, CourseId, SectionId)
│
├── Requests/                          <- Validation DTOs (Data sent from client forms)
│   ├── StudentRequest.cs              <- Validation rules for student create/update
│   ├── TeacherRequest.cs              <- Validation rules for teacher create/update
│   ├── CourseRequest.cs               <- Validation rules for course create/update
│   ├── CourseSectionRequest.cs        <- Validation rules for section create/update
│   └── EnrollmentRequest.cs           <- Validation rules for student enrollment
│
├── Store/                             <- Persistent JSON file storage (Pre-implemented singletons)
│   ├── JsonStudentStore.cs            <- CRUD helper for App_Data/students.json
│   ├── JsonTeacherStore.cs            <- CRUD helper for App_Data/teachers.json
│   ├── JsonCourseStore.cs             <- CRUD helper for App_Data/courses.json
│   ├── JsonSectionStore.cs            <- CRUD helper for App_Data/sections.json
│   └── JsonEnrollmentStore.cs         <- CRUD helper for App_Data/enrollments.json
│
├── App_Data/                          <- Persistent Data Storage
│   ├── students.json                  <- 18 preloaded student records from course roster
│   ├── teachers.json                  <- 719 instructor records
│   ├── courses.json                   <- 1,002 academic course records
│   ├── sections.json                  <- 2,324 active class section records
│   ├── enrollments.json               <- Active enrollments store (starts as empty [])
│   └── university.db                  <- Pre-seeded SQLite database (for optional EF Core track)
│
├── wwwroot/                           <- [YOUR FRONTEND GOES HERE] Static Web Portal
│   ├── index.html                     <- Main web portal HTML skeleton with footer
│   ├── site.css                       <- CSS styling (Flexbox/Grid/Themes)
│   └── app.js                         <- Frontend JavaScript (fetch / DOM rendering)
│
└── Properties/
    └── launchSettings.json            <- Configures Kestrel server to run on port 5086
```

---

## 📄 Explanation of Individual C# Files

### 1. Root & Configuration
- **`Program.cs`**:
  - The entry point of your web application.
  - Registers all five JSON storage singletons (`builder.Services.AddSingleton<JsonStudentStore>()`, etc.) into the ASP.NET Core Dependency Injection (DI) container.
  - Configures Swagger / OpenAPI UI at `/swagger`.
  - Enables `UseDefaultFiles()` and `UseStaticFiles()` to serve your frontend files (`index.html`, `app.js`, `site.css`) from `wwwroot/`.
  - Maps your controller routes with `app.MapControllers()`.

---

### 2. Controllers (`Controllers/`)
*This is where you write your backend C# code.*

- **`HelloWorldController.cs`** *(Pre-built Example)*:
  - Demonstrates how to create a controller using `[ApiController]`, `[Route("api/hello")]`, and `ControllerBase`.
  - Shows how `[HttpGet]` returns HTTP 200 OK with JSON data, and how route parameters work (`[HttpGet("{name}")]`).
  - **Copy this file's structure** to create your own controllers:
    - `StudentsController.cs` (`[Route("api/students")]`)
    - `TeachersController.cs` (`[Route("api/teachers")]`)
    - `CoursesController.cs` (`[Route("api/courses")]`)
    - `SectionsController.cs` (`[Route("api/sections")]`)
    - `EnrollmentsController.cs` (`[Route("api/enrollments")]`)

---

### 3. Domain Models (`Models/`)
*These classes define the shapes of the objects saved in storage and returned by your API.*

- **`Student.cs`**: Represents an enrolled student. Contains `Id`, `FullName`, `Email`, `Major`, `RegistrationNumber`, `FirstName`, `LastName`, `Gender`, `School`, `Program`, and `Department`.
- **`Teacher.cs`**: Represents a faculty member. Contains `Id`, `FullName`, `Email`, `Department`, and `Title` (e.g., Professor, Lecturer).
- **`Course.cs`**: Represents a university course. Contains `Id`, `Code` (e.g. `CSE213`), `Title`, `Credits`, `Department`, `TeacherId` (assigned instructor), and `Description`.
- **`CourseSection.cs`**: Represents a specific scheduled offering of a course. Contains `Id`, `CourseId`, `SectionNumber`, `Semester`, `Room`, `Capacity`, `TeacherId`, and `Department`.
- **`Enrollment.cs`**: Represents a student's registration in a course. Contains `Id`, `StudentId`, `CourseId`, `SectionId` (optional), `EnrollmentDate`, and `Status` (`"Enrolled"`, `"Completed"`, `"Dropped"`).

---

### 4. Validation DTO Requests (`Requests/`)
*These classes represent user input received via HTTP POST or PUT requests. They use Data Annotations to enforce validation rules automatically.*

- **`StudentRequest.cs`**: Validates input when creating or updating a student. Enforces `[Required]` and `[StringLength(100)]` on `FullName`, `[Required]` and `[EmailAddress]` on `Email`, and `[Required]` on `Major`.
- **`TeacherRequest.cs`**: Validates `FullName` (required, max 100), `Email` (required, email format), `Department`, and `Title`.
- **`CourseRequest.cs`**: Validates `Code` (required, 2-12 chars), `Title` (required, max 120), `Credits` (range 1-12), `Department`, and optional `TeacherId`.
- **`CourseSectionRequest.cs`**: Validates `CourseId` (required), `SectionNumber` (required, max 10), `Semester` (required), `Room`, and `Capacity` (range 1-500).
- **`EnrollmentRequest.cs`**: Validates `StudentId` (required), `CourseId` (required), optional `SectionId`, and `Status` (`[RegularExpression("^(Enrolled|Completed|Dropped)$")]`).

> **Tip:** In your controller methods, accepting `[FromBody] StudentRequest request` automatically triggers validation. If a client submits invalid data, ASP.NET Core automatically returns `400 Bad Request` with validation error messages!

---

### 5. Persistent JSON Stores (`Store/`)
*These classes handle reading and saving records to `.json` files on disk in `App_Data/`. They are already fully written and registered for you.*

- **`JsonStudentStore.cs`**:
  - `GetAll()`: Returns all students as `List<Student>`.
  - `GetById(id)`: Finds a student by integer ID, or `null`.
  - `Add(request)`: Creates a new student with an auto-incremented ID and saves to `students.json`.
  - `Update(id, request)`: Updates an existing student record.
  - `Delete(id)`: Removes a student from storage.
  - `Search(query)`: Finds students matching name, email, or major.
- **`JsonTeacherStore.cs`**: Full CRUD operations for `teachers.json`.
- **`JsonCourseStore.cs`**: Full CRUD operations for `courses.json`. Includes filtering by code or search query.
- **`JsonSectionStore.cs`**: Full CRUD operations for `sections.json`. Includes `GetByCourseId(courseId)`.
- **`JsonEnrollmentStore.cs`**: Full CRUD operations for `enrollments.json`. Includes `GetByStudentId(studentId)`, `GetByCourseId(courseId)`, and duplicate check helper `Exists(studentId, courseId)`.

> **How to use stores in your controllers:** Simply inject them into your controller's constructor via Dependency Injection:
> ```csharp
> private readonly JsonStudentStore _studentStore;
> public StudentsController(JsonStudentStore studentStore)
> {
>     _studentStore = studentStore;
> }
> ```

---

## 💡 Beginner's Roadmap (Start Here!)

1. **Step 1 (Verify Setup):** Run `dotnet run` and verify `http://localhost:5086` and `http://localhost:5086/swagger`.
2. **Step 2 (One Controller First):** Create only `Controllers/StudentsController.cs`! Inject `JsonStudentStore` and test `GET /api/students` in Swagger. Then add `POST`, `PUT`, and `DELETE`.
3. **Step 3 (Connect Frontend):** Create `wwwroot/students.html`. Use `api("/api/students")` to fetch and render the 18 students in a table.
4. **Step 4 (Repeat Pattern):** Clone the working pattern for Teachers, Courses, Sections, and Enrollments.
5. **Step 5 (Self-Enroll & Submit):** Put your Name & Reg No in `<footer>`, enroll yourself in a course, take screenshots, and submit your report!

---

## 📖 Official Assignment Handout & Grading Rubric

For the complete technical specifications, exact status codes, 25-row pagination details, report guidelines, and the **"Choose Your Path" extra credit tracks (up to +6%)**, please read the official PDF handout:

👉 **[Assignment/CSE213-Final-Assignment.pdf](../../Assignment/CSE213-Final-Assignment.pdf)**
