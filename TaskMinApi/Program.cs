using Microsoft.EntityFrameworkCore;
using TaskMinApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseCors("AllowReactApp");

// task end-points
app.MapGet("/task", async (DataContext context) =>
    await context.Tasks.ToListAsync());

app.MapGet("/task/{id}", async (DataContext context, int id) =>
    await context.Tasks.FindAsync(id) is Task task ? Results.Ok(task) : Results.NotFound("Sorry, task not found")
);

app.MapPost("/task", async (DataContext context, Task task) =>
{
    context.Tasks.Add(task);
    await context.SaveChangesAsync();
    return Results.Ok(await context.Tasks.ToListAsync());
});

app.MapPut("/task/{id}", async (DataContext context, Task updatedTask, int id) =>
{
    var task = await context.Tasks.FindAsync(id);
    if (task is null)
    {
        return Results.NotFound("Sorry, this task doesn't exist");
    }

    // removed id, because must not be changed
    task.Title = updatedTask.Title;
    task.Description = updatedTask.Description;
    task.Category = updatedTask.Category;
    task.Status = updatedTask.Status;
    task.StartDate = updatedTask.StartDate;
    task.DueDate = updatedTask.DueDate;
    task.AssignedTo = updatedTask.AssignedTo;
    task.AssignedBy = updatedTask.AssignedBy;
    task.Difficulty = updatedTask.Difficulty;
    task.EstimatedTime = updatedTask.EstimatedTime;
    task.ActualTime = updatedTask.ActualTime;
    task.PercentComplete = updatedTask.PercentComplete;
    task.Feedback = updatedTask.Feedback;
    await context.SaveChangesAsync();

    return Results.Ok(await context.Tasks.ToListAsync());
});

app.MapDelete("/task/{id}", async (DataContext context, int id) =>
{
    var task = await context.Tasks.FindAsync(id);
    if (task is null)
    {
        return Results.NotFound("Sorry, this task doesn't exist");
    }

    context.Tasks.Remove(task);
    await context.SaveChangesAsync();
    return Results.Ok(await context.Tasks.ToListAsync());
});

// user end-points

app.MapGet("/user", async (DataContext context) =>
    await context.Users.ToListAsync());

app.MapGet("/user/{id}", async (DataContext context, int id) =>
    await context.Users.FindAsync(id) is User user ? Results.Ok(user) : Results.NotFound("Sorry, user not found"));

app.MapPost("/user", async (DataContext context, User user) =>
{
    context.Users.Add(user);
    await context.SaveChangesAsync();
    return Results.Ok(await context.Users.ToListAsync());
});
app.MapPut("/user/{id}", async (DataContext context, User updatedUser, int id) =>
{
    var user = await context.Users.FindAsync(id);
    if (user is null)
        return Results.NotFound("Sorry, this user does not exist");

    // UserId should not be updateable
    user.UserName = updatedUser.UserName;
    user.Cohort = updatedUser.Cohort;
    user.Email = updatedUser.Email;
    user.Role = updatedUser.Role;
    user.Team = updatedUser.Team;
    user.FirebaseUID = updatedUser.FirebaseUID;

    await context.SaveChangesAsync();

    return Results.Ok(await context.Users.ToListAsync());
});
app.MapDelete("/user/{id}", async (DataContext context, int id) =>
{
    var user = await context.Tasks.FindAsync(id);
    if (user is null)
    {
        return Results.NotFound("Sorry, this user doesn't exist");
    }

    context.Tasks.Remove(user);
    await context.SaveChangesAsync();
    return Results.Ok(await context.Users.ToListAsync());
});

app.Run();

public class Task
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public required string Status { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime DueDate { get; set; }
    public string AssignedTo { get; set; }
    public string AssignedBy { get; set; }
    public string Difficulty { get; set; }
    public int EstimatedTime { get; set; }
    public int ActualTime { get; set; }
    public double PercentComplete { get; set; }
    public string Feedback { get; set; }
}

public class User
{
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string Cohort { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public string Team { get; set; }
    public string FirebaseUID { get; set; }
}