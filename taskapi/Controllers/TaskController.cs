using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;

[ApiController]
[Route("[controller]")]
public class TaskController : ControllerBase
{
    private readonly TaskContext _context;
    public TaskController(TaskContext context)
    {
        _context = context;
    }

    [HttpPost]
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

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskItem>> GetTaskbyId(int id)
    {
        var item = await _context.TodoItems.FindAsync(id);
        if (item == null)
            return NotFound();
        return item;
    }
}
