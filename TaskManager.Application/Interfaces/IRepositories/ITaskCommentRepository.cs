using TaskManager.Domain.Entities;
namespace TaskManager.Application.Interfaces.IRepositories
{
    public interface ITaskCommentRepository : IRepository<TaskComment>
    {
        Task<IEnumerable<TaskComment>> GetByTaskIdAsync(int taskId);
    }
}