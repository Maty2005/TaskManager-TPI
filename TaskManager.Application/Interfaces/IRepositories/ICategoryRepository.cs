using TaskManager.Domain.Entities;

namespace TaskManager.Application.Interfaces.IRepositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category?> GetByNameAsync(string name);
    }
}
