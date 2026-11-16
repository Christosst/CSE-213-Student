# How to Run CourseApi

## Prerequisites

- Install the [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0).
- Install [Visual Studio Code](https://code.visualstudio.com/).
- Open the `CSE-213-Student` folder in Visual Studio Code.

## Run from the VS Code terminal

If your terminal is currently at `G:\GitHub\CSE213`, run:

```powershell
dotnet run --project ".\CSE-213-Student\05-ASPNet\CourseApi\CourseApi.csproj" --launch-profile classroom
```

If you opened the `CSE-213-Student` folder in Visual Studio Code, run:

```powershell
dotnet run --project ".\05-ASPNet\CourseApi\CourseApi.csproj" --launch-profile classroom
```

```powershell
dotnet run --project ".\05-ASPNet\CourseApi\CourseApi.csproj" --launch-profile classroom
```

The API will start at:

- Website: <http://localhost:5080/>
- Swagger UI: <http://localhost:5080/swagger>

## Stop the project

Press **Ctrl+C** in the terminal.

## Run from the project folder

Alternatively, open a terminal in the `CourseApi` folder and run:

```powershell
dotnet run --launch-profile classroom
```

