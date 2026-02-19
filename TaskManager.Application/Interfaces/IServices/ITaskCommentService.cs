using TaskManager.Application.DTOs;
namespace TaskManager.Application.Interfaces.IServices
{
    public interface ITaskCommentService
    {
        Task<IEnumerable<CommentResponseDto>> GetTaskCommentsAsync(int taskId);
        Task<CommentResponseDto> AddCommentAsync(int taskId, CreateCommentDto createCommentDto, int userId);
        Task DeleteCommentAsync(int commentId, int userId);
    }
}