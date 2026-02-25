using TaskManager.Domain.Common;
using TaskManager.Domain.Enums;
using TaskStatusEnum = TaskManager.Domain.Enums.TaskItemStatus;
namespace TaskManager.Domain.Entities
{
    public class TaskItem : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TaskStatusEnum Status { get; set; } = TaskStatusEnum.Pending;

        public Priority Priority { get; set; } = Priority.Medium;
        public DateTime? DueDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public int UserId { get; set; }
        public int? CategoryId { get; set; }
        public virtual User User { get; set; } = null!;
        public virtual Category? Category { get; set; }
        public virtual ICollection<TaskComment> Comments { get; set; } = new List<TaskComment>();
    }
}
