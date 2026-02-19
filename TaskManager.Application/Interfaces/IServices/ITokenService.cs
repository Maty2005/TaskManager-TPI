using TaskManager.Domain.Entities;
namespace TaskManager.Application.Interfaces.IServices
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}