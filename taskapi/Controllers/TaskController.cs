using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

[ApiController]
[Route("api/[controller]")]
public class TaskController : ControllerBase
{
    private readonly TaskContext _context;
    public TaskController(TaskContext context)
    {
        _context = context;
    }

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

        return CreatedAtAction(nameof(GetTaskbyId), new { id = item.Id }, item);
    }

    [HttpGet("gettaskbyid/{id}")]
    public async Task<ActionResult<TaskItem>> GetTaskbyId(int id)
    {
        var item = await _context.TodoItems.FindAsync(id);
        if (item == null)
            return NotFound();
        return item;
    }

    [HttpGet("getalltasks")]
    public async Task<ActionResult<IEnumerable<TaskItem>>> GetAllTasks()
    {
        var items = await _context.TodoItems.ToListAsync();
        return Ok(items);
    }

    [HttpPatch("updatetaskstatus/{id}")]
    public async Task<IActionResult> UpdateTaskStatus(int id, [FromBody] UpdateTaskStatusDto dto)
    {
        var item = await _context.TodoItems.FindAsync(id);
        if (item == null)
            return NotFound();

        item.IsCompleted = dto.IsCompleted;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("deletetask/{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var item = await _context.TodoItems.FindAsync(id);
        if (item == null)
            return NotFound();

        _context.TodoItems.Remove(item);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
