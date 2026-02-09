namespace TaskManager.Application.Interfaces.IServices
{
    public interface IQuoteService
    {
        Task<string> GetDailyQuoteAsync();
    }
}