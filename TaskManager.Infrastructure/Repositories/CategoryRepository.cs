using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Interfaces.IRepositories;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Data;
namespace TaskManager.Infrastructure.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<Category?> GetByNameAsync(string name)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.Name == name);
        }
        public override async Task<Category?> GetByIdAsync(int id)
        {
            return await _dbSet
            .Include(c => c.Tasks)
            .FirstOrDefaultAsync(c => c.Id == id);
        }
        public override async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _dbSet
            .Include(c => c.Tasks)
            .ToListAsync();
        }
    }
}