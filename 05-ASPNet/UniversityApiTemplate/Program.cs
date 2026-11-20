using UniversityApiTemplate;

var builder = WebApplication.CreateBuilder(args);

// 1. Register API controllers and OpenAPI documentation
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// 2. Register persistent JSON stores as singletons
builder.Services.AddSingleton<JsonStudentStore>();
builder.Services.AddSingleton<JsonTeacherStore>();
builder.Services.AddSingleton<JsonCourseStore>();
builder.Services.AddSingleton<JsonSectionStore>();
builder.Services.AddSingleton<JsonEnrollmentStore>();

var app = builder.Build();

// 3. Enable Swagger UI
app.MapOpenApi();
app.UseSwaggerUI(options =>
    options.SwaggerEndpoint("/openapi/v1.json", "University API (Student Template)"));

// 4. Serve frontend static files from wwwroot
app.UseDefaultFiles();
app.UseStaticFiles();

// 5. Map controllers
app.MapControllers();

app.Run();
