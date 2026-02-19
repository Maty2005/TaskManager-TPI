using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Interfaces.IRepositories;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Data;
namespace TaskManager.Infrastructure.Repositories
{
    public class TaskCommentRepository : Repository<TaskComment>, ITaskCommentRepository
    {
        public TaskCommentRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<TaskComment>> GetByTaskIdAsync(int taskId)
        {
            return await _dbSet
            .Include(c => c.User)
            .Include(c => c.Task)
            .Where(c => c.TaskId == taskId)
            .OrderBy(c => c.CreatedAt)
            .ToListAsync();
        }
        public override async Task<TaskComment?> GetByIdAsync(int id)
        {
            return await _dbSet
            .Include(c => c.User)
            .Include(c => c.Task)
            .FirstOrDefaultAsync(c => c.Id == id);
        }
        public override async Task<IEnumerable<TaskComment>> GetAllAsync()
        {
            return await _dbSet
            .Include(c => c.User)
            .Include(c => c.Task)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
        }
    }
}