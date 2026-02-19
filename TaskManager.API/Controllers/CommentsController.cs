using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces.IServices;
namespace TaskManager.API.Controllers
{
    [Route("api/tasks/{taskId}/comments")]
    [ApiController]
    [Authorize]
    public class CommentsController : ControllerBase
    {
        private readonly ITaskCommentService _commentService;
        public CommentsController(ITaskCommentService commentService)
        {
            _commentService = commentService;
        }
        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new UnauthorizedAccessException("Usuario no autenticado");
            return int.Parse(userIdClaim.Value);
        }
        [HttpGet]
        public async Task<IActionResult> GetTaskComments(int taskId)
        {
            try
            {
                var comments = await _commentService.GetTaskCommentsAsync(taskId);
                return Ok(comments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener comentarios", error = ex.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddComment(int taskId, [FromBody] CreateCommentDto createCommentDto)
        {
            try
            {
                var userId = GetUserId();
                var comment = await _commentService.AddCommentAsync(taskId, createCommentDto, userId);
                return CreatedAtAction(nameof(GetTaskComments), new { taskId }, comment);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al crear comentario", error = ex.Message });
            }
        }
        [HttpDelete("~/api/comments/{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            try
            {
                var userId = GetUserId();
                await _commentService.DeleteCommentAsync(id, userId);
                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al eliminar comentario", error = ex.Message });
            }
        }
    }
}