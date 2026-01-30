using Microsoft.EntityFrameworkCore;
using TodoApi.Models; // Adjust namespace as needed

public class TaskContext : DbContext
{
    public TaskContext(DbContextOptions<TaskContext> options) : base(options) { }

    public DbSet<TodoApi.Models.TaskItem> TodoItems { get; set; }
}
