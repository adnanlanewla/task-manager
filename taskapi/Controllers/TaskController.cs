using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TaskApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskController : ControllerBase
{
    private readonly TaskContext _context;
    public TaskController(TaskContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Creates a new task with the provided title and completion status.
    /// </summary>
    [HttpPost("createtask")]
    public async Task<ActionResult<TaskItem>> CreateTask([FromBody] CreateTaskDto dto)
    {
        var item = new TodoApi.Models.TaskItem
        {
            Title = dto.Title,
            IsCompleted = dto.IsCompleted
        };

        _context.TodoItems.Add(item);
        await _context.SaveChangesAsync();
        // Returns 201 Created with the location of the new task.
        return CreatedAtAction(nameof(GetTaskbyId), new { id = item.Id }, item);
    }


    /// <summary>
    /// Retrieves a task by its unique ID.
    /// Returns 404 if the task does not exist.
    /// </summary>
    [HttpGet("gettaskbyid/{id}")]
    public async Task<ActionResult<TaskItem>> GetTaskbyId(int id)
    {
        var item = await _context.TodoItems.FindAsync(id);
        if (item == null)
            return NotFound();
        return item;
    }

    /// <summary>
    /// Retrieves all tasks in the database.
    /// </summary>
    [HttpGet("getalltasks")]
    public async Task<ActionResult<IEnumerable<TaskItem>>> GetAllTasks()
    {
        var items = await _context.TodoItems.ToListAsync();
        return Ok(items);
    }

    /// <summary>
    /// Updates the completion status of a task by its ID.
    /// Returns 404 if the task does not exist.
    /// </summary>
    [HttpPatch("updatetaskstatus/{id}")]
    public async Task<IActionResult> UpdateTaskStatus(int id, [FromBody] UpdateTaskStatusDto dto)
    {
        var item = await _context.TodoItems.FindAsync(id);
        if (item == null)
            return NotFound();

        item.IsCompleted = dto.IsCompleted;
        await _context.SaveChangesAsync();
        // Returns 204 No Content on successful update.
        return NoContent();
    }

    /// <summary>
    /// Deletes a task by its ID.
    /// Returns 404 if the task does not exist.
    /// </summary>
    [HttpDelete("deletetask/{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var item = await _context.TodoItems.FindAsync(id);
        if (item == null)
            return NotFound();

        _context.TodoItems.Remove(item);
        await _context.SaveChangesAsync();
        // Returns 204 No Content on successful deletion.
        return NoContent();
    }
}
