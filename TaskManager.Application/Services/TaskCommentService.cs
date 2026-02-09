using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces.IRepositories;
using TaskManager.Application.Interfaces.IServices;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Services
{
    public class TaskCommentService : ITaskCommentService
    {
        private readonly ITaskCommentRepository _commentRepository;
        private readonly ITaskRepository _taskRepository;

        public TaskCommentService(ITaskCommentRepository commentRepository, ITaskRepository taskRepository)
        {
            _commentRepository = commentRepository;
            _taskRepository = taskRepository;
        }

        public async Task<IEnumerable<CommentResponseDto>> GetTaskCommentsAsync(int taskId)
        {
            var comments = await _commentRepository.GetByTaskIdAsync(taskId);
            return comments.Select(MapToResponseDto);
        }

        public async Task<CommentResponseDto> AddCommentAsync(int taskId, CreateCommentDto createCommentDto, int userId)
        {
            var task = await _taskRepository.GetByIdAsync(taskId);
            if (task == null)
                throw new InvalidOperationException("La tarea no existe");

            var comment = new TaskComment
            {
                Content = createCommentDto.Content,
                TaskId = taskId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            var createdComment = await _commentRepository.AddAsync(comment);
            var result = await _commentRepository.GetByIdAsync(createdComment.Id);

            return MapToResponseDto(result!);
        }

        public async Task DeleteCommentAsync(int commentId, int userId)
        {
            var comment = await _commentRepository.GetByIdAsync(commentId);

            if (comment == null)
                throw new InvalidOperationException("Comentario no encontrado");

            if (comment.UserId != userId)
                throw new UnauthorizedAccessException("No tienes permiso para eliminar este comentario");

            await _commentRepository.DeleteAsync(commentId);
        }

        private CommentResponseDto MapToResponseDto(TaskComment comment)
        {
            return new CommentResponseDto
            {
                Id = comment.Id,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
                UserId = comment.UserId,
                Username = comment.User?.Username ?? string.Empty,
                TaskId = comment.TaskId
            };
        }
    }
}