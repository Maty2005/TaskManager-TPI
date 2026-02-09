using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces.IRepositories;
using TaskManager.Application.Interfaces.IServices;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using DomainTaskStatus = TaskManager.Domain.Enums.TaskStatus;

namespace TaskManager.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ICategoryRepository _categoryRepository;

        public TaskService(ITaskRepository taskRepository, ICategoryRepository categoryRepository)
        {
            _taskRepository = taskRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<TaskResponseDto>> GetUserTasksAsync(int userId)
        {
            var tasks = await _taskRepository.GetByUserIdAsync(userId);
            return tasks.Select(MapToResponseDto);
        }

        public async Task<TaskResponseDto?> GetTaskByIdAsync(int taskId, int userId)
        {
            var task = await _taskRepository.GetByIdAsync(taskId);

            if (task == null || task.UserId != userId)
                return null;

            return MapToResponseDto(task);
        }

        public async Task<TaskResponseDto> CreateTaskAsync(CreateTaskDto createTaskDto, int userId)
        {
            if (createTaskDto.CategoryId.HasValue)
            {
                var categoryExists = await _categoryRepository.ExistsAsync(createTaskDto.CategoryId.Value);
                if (!categoryExists)
                    throw new InvalidOperationException("La categoría especificada no existe");
            }

            var task = new TaskItem
            {
                Title = createTaskDto.Title,
                Description = createTaskDto.Description,
                Priority = createTaskDto.Priority,
                DueDate = createTaskDto.DueDate,
                CategoryId = createTaskDto.CategoryId,
                UserId = userId,
                Status = DomainTaskStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdTask = await _taskRepository.AddAsync(task);
            var result = await _taskRepository.GetByIdAsync(createdTask.Id);

            return MapToResponseDto(result!);
        }

        public async Task<TaskResponseDto> UpdateTaskAsync(int taskId, UpdateTaskDto updateTaskDto, int userId)
        {
            var task = await _taskRepository.GetByIdAsync(taskId);

            if (task == null || task.UserId != userId)
                throw new UnauthorizedAccessException("No tienes permiso para actualizar esta tarea");

            if (!string.IsNullOrWhiteSpace(updateTaskDto.Title))
                task.Title = updateTaskDto.Title;

            if (updateTaskDto.Description != null)
                task.Description = updateTaskDto.Description;

            if (updateTaskDto.Status.HasValue)
                task.Status = (DomainTaskStatus)updateTaskDto.Status.Value;

            if (updateTaskDto.Priority.HasValue)
                task.Priority = updateTaskDto.Priority.Value;

            if (updateTaskDto.DueDate.HasValue)
                task.DueDate = updateTaskDto.DueDate.Value;

            if (updateTaskDto.CategoryId.HasValue)
            {
                var categoryExists = await _categoryRepository.ExistsAsync(updateTaskDto.CategoryId.Value);
                if (!categoryExists)
                    throw new InvalidOperationException("La categoría especificada no existe");
                task.CategoryId = updateTaskDto.CategoryId.Value;
            }

            task.UpdatedAt = DateTime.UtcNow;
            await _taskRepository.UpdateAsync(task);

            var updatedTask = await _taskRepository.GetByIdAsync(taskId);
            return MapToResponseDto(updatedTask!);
        }

        public async Task DeleteTaskAsync(int taskId, int userId)
        {
            var task = await _taskRepository.GetByIdAsync(taskId);

            if (task == null || task.UserId != userId)
                throw new UnauthorizedAccessException("No tienes permiso para eliminar esta tarea");

            await _taskRepository.DeleteAsync(taskId);
        }

        public async Task<TaskResponseDto> UpdateTaskStatusAsync(int taskId, int status, int userId)
        {
            var task = await _taskRepository.GetByIdAsync(taskId);

            if (task == null || task.UserId != userId)
                throw new UnauthorizedAccessException("No tienes permiso para actualizar esta tarea");

            task.Status = (DomainTaskStatus)status;
            task.UpdatedAt = DateTime.UtcNow;

            await _taskRepository.UpdateAsync(task);

            var updatedTask = await _taskRepository.GetByIdAsync(taskId);
            return MapToResponseDto(updatedTask!);
        }

        private TaskResponseDto MapToResponseDto(TaskItem task)
        {
            return new TaskResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                Priority = task.Priority,
                DueDate = task.DueDate,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt,
                UserId = task.UserId,
                Username = task.User?.Username ?? string.Empty,
                CategoryId = task.CategoryId,
                CategoryName = task.Category?.Name,
                CommentsCount = task.Comments?.Count ?? 0
            };
        }
    }
}