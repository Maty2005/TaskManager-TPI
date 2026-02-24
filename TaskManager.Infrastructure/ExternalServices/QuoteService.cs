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
        }

        public async Task<string> GetDailyQuoteAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("https://zenquotes.io/api/random");

                if (!response.IsSuccessStatusCode)
                {
                    return "No se pudo obtener la frase del día. Intenta nuevamente más tarde.";
                }

                var jsonString = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var quotes = JsonSerializer.Deserialize<List<ZenQuote>>(jsonString, options);

                if (quotes == null || quotes.Count == 0)
                {
                    return "No se pudo procesar la frase del día.";
                }

                return $"{quotes[0].Q} - {quotes[0].A}";
            }
            catch (Exception ex)
            {
                return $"Error al obtener la frase: {ex.Message}";
            }
        }

        private class ZenQuote
        {
            public string Q { get; set; } = string.Empty;
            public string A { get; set; } = string.Empty;
        }
    }
}
