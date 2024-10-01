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


app.MapGet("/task", async (DataContext context) =>
    await context.Tasks.ToListAsync());

app.MapGet("/task/{id}", async (DataContext context, int id) =>
    await context.Tasks.FindAsync(id) is Task task ? Results.Ok(task) : Results.NotFound("Sorry, book not found")
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

    task.Title = updatedTask.Title;
    task.Description = updatedTask.Description;
    task.Status = updatedTask.Status;
    task.Duedate = updatedTask.Duedate;
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

app.Run();

public class Task
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Status { get; set; }
    public required DateTime Duedate { get; set; }
}