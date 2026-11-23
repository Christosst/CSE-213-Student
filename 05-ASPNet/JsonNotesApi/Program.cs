using JsonNotesApi;

var builder = WebApplication.CreateBuilder(args);

// 1. Register services
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSingleton<JsonNoteStore>();

var app = builder.Build();

// 2. Enable Swagger UI to test the API in the browser
app.MapOpenApi();
app.UseSwaggerUI(options =>
    options.SwaggerEndpoint("/openapi/v1.json", "JSON Notes API"));

// 3. Serve frontend static files (HTML, CSS, JS) from wwwroot
app.UseDefaultFiles();
app.UseStaticFiles();

// 4. Map API controllers
app.MapControllers();

app.Run();
