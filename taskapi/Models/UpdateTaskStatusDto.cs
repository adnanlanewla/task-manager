
// DTO used to update only the completion status of a task.
// Keeps update operations clear and separate from task creation.
public class UpdateTaskStatusDto
{
    public bool IsCompleted { get; set; }
}
