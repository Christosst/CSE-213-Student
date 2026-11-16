namespace CourseApi;

// In-memory storage for tasks.
// Data is stored in a simple C# List and will reset whenever the server restarts.
public class TaskStore
{
    // A list to hold all tasks in memory
    private readonly List<TaskItem> tasks = new();

    // A counter to assign a unique ID to each new task
    private int nextId = 1;

    // Get all tasks, with an optional filter for completed status (true or false)
    public List<TaskItem> GetAll(bool? completed = null)
    {
        // If the caller did not specify a completed filter, return all tasks
        if (completed == null)
        {
            return tasks;
        }

        // Otherwise, filter tasks manually so beginner students can follow the logic
        var filteredList = new List<TaskItem>();
        foreach (var task in tasks)
        {
            if (task.IsCompleted == completed.Value)
            {
                filteredList.Add(task);
            }
        }

        return filteredList;
    }

    // Find a single task by its unique ID
    public TaskItem? GetById(int id)
    {
        // Loop through each task to find the matching ID
        foreach (var task in tasks)
        {
            if (task.Id == id)
            {
                return task; // Found!
            }
        }

        return null; // Not found
    }

    // Add a new task to the list
    public TaskItem Add(TaskWriteRequest request)
    {
        // Create a new TaskItem object
        var task = new TaskItem
        {
            Id = nextId,
            Title = request.Title.Trim(),
            IsCompleted = request.IsCompleted
        };

        // Increase nextId by 1 so the next task gets a new unique ID
        nextId = nextId + 1;

        // Add the task to our in-memory list
        tasks.Add(task);

        return task;
    }

    // Update an existing task's title and completed status
    public bool Update(int id, TaskWriteRequest request)
    {
        // Look for the task first
        var task = GetById(id);
        if (task == null)
        {
            return false; // Task does not exist
        }

        // Update the task properties
        task.Title = request.Title.Trim();
        task.IsCompleted = request.IsCompleted;
        return true; // Successfully updated
    }

    // Delete a task by its ID
    public bool Delete(int id)
    {
        // Look for the task first
        var task = GetById(id);
        if (task == null)
        {
            return false; // Task does not exist
        }

        // Remove the task from our list
        tasks.Remove(task);
        return true; // Successfully deleted
    }
}
