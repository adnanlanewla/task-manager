using Xunit;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using TaskApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace TaskApi.Tests;

public class TaskControllerTests
{
    // Creates a new in-memory SQLite context for isolated testing.
    private TaskContext GetSqliteDbContext()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<TaskContext>()
            .UseSqlite(connection)
            .Options;

        var context = new TaskContext(options);
        context.Database.EnsureCreated();
        return context;
    }

    // Tests that creating a task returns the created task with correct values.
    [Fact]
    public async Task CreateTask_ReturnsCreatedTask()
    {
        var context = GetSqliteDbContext();
        var controller = new TaskController(context);
        var dto = new CreateTaskDto { Title = "Test Task", IsCompleted = false };

        var result = await controller.CreateTask(dto);

        // Assert that the result is a CreatedAtActionResult with the correct task data.
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var task = Assert.IsType<TaskItem>(createdResult.Value);
        Assert.Equal("Test Task", task.Title);
        Assert.False(task.IsCompleted);
    }

    // Tests that requesting a non-existent task by ID returns NotFound.
    [Fact]
    public async Task GetTaskById_ReturnsNotFound_WhenTaskDoesNotExist()
    {
        var context = GetSqliteDbContext();
        var controller = new TaskController(context);

        var result = await controller.GetTaskbyId(1);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    // Tests that retrieving all tasks returns all existing tasks.
    [Fact]
    public async Task GetAllTasks_ReturnsAllTasks()
    {
        var context = GetSqliteDbContext();
        context.TodoItems.Add(new TaskItem { Title = "Task 1", IsCompleted = false });
        context.TodoItems.Add(new TaskItem { Title = "Task 2", IsCompleted = true });
        context.SaveChanges();

        var controller = new TaskController(context);

        var result = await controller.GetAllTasks();

        // Assert that both tasks are returned in the response.
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var tasks = Assert.IsAssignableFrom<IEnumerable<TaskItem>>(okResult.Value);
        Assert.Collection(tasks,
            t => Assert.Equal("Task 1", t.Title),
            t => Assert.Equal("Task 2", t.Title)
        );
    }

    // Tests that updating a task's status actually updates the IsCompleted property.
    [Fact]
    public async Task UpdateTaskStatus_UpdatesStatus()
    {
        var context = GetSqliteDbContext();
        var task = new TaskItem { Title = "Task", IsCompleted = false };
        context.TodoItems.Add(task);
        context.SaveChanges();

        var controller = new TaskController(context);
        var dto = new UpdateTaskStatusDto { IsCompleted = true };

        var result = await controller.UpdateTaskStatus(task.Id, dto);

        Assert.IsType<NoContentResult>(result);
        // Verify the status was updated in the database.
        Assert.True(context.TodoItems.Find(task.Id).IsCompleted);
    }

    // Tests that deleting a task removes it from the database.
    [Fact]
    public async Task DeleteTask_RemovesTask()
    {
        var context = GetSqliteDbContext();
        var task = new TaskItem { Title = "Task", IsCompleted = false };
        context.TodoItems.Add(task);
        context.SaveChanges();

        var controller = new TaskController(context);

        var result = await controller.DeleteTask(task.Id);

        Assert.IsType<NoContentResult>(result);
        // Confirm the task no longer exists.
        Assert.Null(context.TodoItems.Find(task.Id));
    }
}
