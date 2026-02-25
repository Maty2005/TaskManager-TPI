using TaskManager.Domain.Common;
namespace TaskManager.Domain.Entities
{
    public class TaskComment : BaseEntity
    {
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int TaskId { get; set; }
        public int UserId { get; set; }
        public virtual TaskItem Task { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
