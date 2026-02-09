using System.ComponentModel.DataAnnotations;
using TaskManager.Domain.Enums;

namespace TaskManager.Application.DTOs
{
    public class UpdateTaskDto
    {
        [StringLength(200, MinimumLength = 3)]
        public string? Title { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        public TaskManager.Domain.Enums.TaskStatus? Status { get; set; }

        public Priority? Priority { get; set; }

        public DateTime? DueDate { get; set; }

        public int? CategoryId { get; set; }
    }
}