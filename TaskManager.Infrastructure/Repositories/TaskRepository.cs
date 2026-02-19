using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Interfaces.IRepositories;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Infrastructure.Data;
namespace TaskManager.Infrastructure.Repositories
{
    public class TaskRepository : Repository<TaskItem>, ITaskRepository
    {
        public TaskRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<TaskItem>> GetByUserIdAsync(int userId)
        {
            return await _dbSet
                .Include(t => t.User)
                .Include(t => t.Category)
                .Include(t => t.Comments)
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetByCategoryIdAsync(int categoryId)
        {
            return await _dbSet
                .Include(t => t.User)
                .Include(t => t.Category)
                .Include(t => t.Comments)
                .Where(t => t.CategoryId == categoryId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetByStatusAsync(int userId, TaskItemStatus status)
        {
            return await _dbSet
                .Include(t => t.User)
                .Include(t => t.Category)
                .Include(t => t.Comments)
                .Where(t => t.UserId == userId && t.Status == status)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }
    }
}
