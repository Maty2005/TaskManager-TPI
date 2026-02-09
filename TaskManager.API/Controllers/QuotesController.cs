using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Interfaces.IServices;

namespace TaskManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QuotesController : ControllerBase
    {
        private readonly IQuoteService _quoteService;

        public QuotesController(IQuoteService quoteService)
        {
            _quoteService = quoteService;
        }

        [HttpGet("daily")]
        public async Task<IActionResult> GetDailyQuote()
        {
            try
            {
                var quote = await _quoteService.GetDailyQuoteAsync();
                return Ok(new { quote, date = DateTime.UtcNow.ToString("yyyy-MM-dd") });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener frase", error = ex.Message });
            }
        }
    }
}