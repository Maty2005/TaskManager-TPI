using System.Text.Json;
using TaskManager.Application.Interfaces.IServices;
using TaskManager.Infrastructure.ExternalServices.Models;
namespace TaskManager.Infrastructure.ExternalServices
{
    public class QuoteService : IQuoteService
    {
        private readonly HttpClient _httpClient;
        public QuoteService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://zenquotes.io/api/random");
        }
        public async Task<string> GetDailyQuoteAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("random");

                if (!response.IsSuccessStatusCode)
                {
                    return "No se pudo obtener la frase del día. Intenta nuevamente más tarde.";
                }
                var jsonString = await response.Content.ReadAsStringAsync();
                var quote = JsonSerializer.Deserialize<QuoteResponse>(jsonString);
                if (quote == null || string.IsNullOrEmpty(quote.content))
                {
                    return "No se pudo procesar la frase del día.";
                }
                return $"{quote.content} - {quote.author}";
            }
            catch (Exception ex)
            {
                return $"Error al obtener la frase: {ex.Message}";
            }
        }
    }
}
