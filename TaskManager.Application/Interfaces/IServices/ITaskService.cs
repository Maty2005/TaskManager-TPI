using TaskManager.Application.DTOs;
namespace TaskManager.Application.Interfaces.IServices
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskResponseDto>> GetUserTasksAsync(int userId);
        Task<TaskResponseDto?> GetTaskByIdAsync(int taskId, int userId);
        Task<TaskResponseDto> CreateTaskAsync(CreateTaskDto createTaskDto, int userId);
        Task<TaskResponseDto> UpdateTaskAsync(int taskId, UpdateTaskDto updateTaskDto, int userId);
        Task DeleteTaskAsync(int taskId, int userId);
        Task<TaskResponseDto> UpdateTaskStatusAsync(int taskId, int status, int userId);
    }
}
