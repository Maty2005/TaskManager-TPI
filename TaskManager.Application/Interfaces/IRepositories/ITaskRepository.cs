using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
namespace TaskManager.Application.Interfaces.IRepositories
{
    public interface ITaskRepository : IRepository<TaskItem>
    {
        Task<IEnumerable<TaskItem>> GetByUserIdAsync(int userId);
        Task<IEnumerable<TaskItem>> GetByCategoryIdAsync(int categoryId);
        Task<IEnumerable<TaskItem>> GetByStatusAsync(int userId, TaskItemStatus status);

    }
}