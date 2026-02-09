using TaskManager.Application.DTOs;

namespace TaskManager.Application.Interfaces.IServices
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResponseDto>> GetAllCategoriesAsync();
        Task<CategoryResponseDto?> GetCategoryByIdAsync(int id);
        Task<CategoryResponseDto> CreateCategoryAsync(CreateCategoryDto createCategoryDto);
        Task<CategoryResponseDto> UpdateCategoryAsync(int id, CreateCategoryDto updateCategoryDto);
        Task DeleteCategoryAsync(int id);
    }
}