using Microsoft.EntityFrameworkCore;
using TaskMinApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer(); // Enables API endpoint documentation
builder.Services.AddSwaggerGen(); // Adds Swagger for API documentation
builder.Services.AddDbContext<DataContext>(); // Registers DataContext for Entity Framework
builder.Services.AddCors(options =>
{
    // Configure CORS to allow requests from the React app
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // Allow only this origin
            .AllowAnyHeader() // Allow any header
            .AllowAnyMethod(); // Allow any HTTP method
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Enable Swagger only in development mode for API documentation
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection(); // Optionally enable HTTPS redirection
app.UseCors("AllowReactApp"); // Enable CORS for the specified policy

// Endpoint to get all tasks
app.MapGet("/task", async (DataContext context) =>
    await context.Tasks.ToListAsync());

// Endpoint to get a task by ID
app.MapGet("/task/{id}", async (DataContext context, int id) =>
    await context.Tasks.FindAsync(id) is Task task ? Results.Ok(task) : Results.NotFound("Sorry, task not found")
);

// Endpoint to add a new task
app.MapPost("/task", async (DataContext context, Task task) =>
{
    context.Tasks.Add(task); // Add new task to the database
    await context.SaveChangesAsync(); // Save changes to the database
    return Results.Ok(await context.Tasks.ToListAsync()); // Return updated list of tasks
});

// Endpoint to update an existing task
app.MapPut("/task/{id}", async (DataContext context, Task updatedTask, int id) =>
{
    var task = await context.Tasks.FindAsync(id); // Find the task by ID
    if (task is null)
    {
        return Results.NotFound("Sorry, this task doesn't exist"); // Return 404 if task not found
    }

    // Update task properties with new values
    task.Title = updatedTask.Title;
    task.Description = updatedTask.Description;
    task.Status = updatedTask.Status;
    task.Duedate = updatedTask.Duedate;
    await context.SaveChangesAsync(); // Save changes to the database

    return Results.Ok(await context.Tasks.ToListAsync()); // Return updated list of tasks
});

// Endpoint to delete a task by ID
app.MapDelete("/task/{id}", async (DataContext context, int id) =>
{
    var task = await context.Tasks.FindAsync(id); // Find the task by ID
    if (task is null)
    {
        return Results.NotFound("Sorry, this task doesn't exist"); // Return 404 if task not found
    }

    context.Tasks.Remove(task); // Remove the task from the database
    await context.SaveChangesAsync(); // Save changes to the database
    return Results.Ok(await context.Tasks.ToListAsync()); // Return updated list of tasks
});

app.Run(); // Start the application

// Task class representing the data model
public class Task
{
    public int Id { get; set; } // Unique identifier for each task
    public required string Title { get; set; } // Title of the task (required)
    public required string Description { get; set; } // Description of the task (required)
    public required string Status { get; set; } // Status of the task (e.g., Pending, Completed) (required)
    public required DateTime Duedate { get; set; } // Due date for the task (required)
}
