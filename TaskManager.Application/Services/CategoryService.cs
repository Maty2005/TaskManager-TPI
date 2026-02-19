using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces.IRepositories;
using TaskManager.Application.Interfaces.IServices;
using TaskManager.Domain.Entities;
namespace TaskManager.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<IEnumerable<CategoryResponseDto>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return categories.Select(MapToResponseDto);
        }
        public async Task<CategoryResponseDto?> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            return category != null ? MapToResponseDto(category) : null;
        }
        public async Task<CategoryResponseDto> CreateCategoryAsync(CreateCategoryDto createCategoryDto)
        {
            var existingCategory = await _categoryRepository.GetByNameAsync(createCategoryDto.Name);
            if (existingCategory != null)
                throw new InvalidOperationException("Ya existe una categoría con ese nombre");
            var category = new Category
            {
                Name = createCategoryDto.Name,
                Description = createCategoryDto.Description,
                Color = createCategoryDto.Color
            };
            var createdCategory = await _categoryRepository.AddAsync(category);
            return MapToResponseDto(createdCategory);
        }
        public async Task<CategoryResponseDto> UpdateCategoryAsync(int id, CreateCategoryDto updateCategoryDto)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                throw new InvalidOperationException("Categoría no encontrada");
            var existingCategory = await _categoryRepository.GetByNameAsync(updateCategoryDto.Name);
            if (existingCategory != null && existingCategory.Id != id)
                throw new InvalidOperationException("Ya existe otra categoría con ese nombre");
            category.Name = updateCategoryDto.Name;
            category.Description = updateCategoryDto.Description;
            category.Color = updateCategoryDto.Color;
            await _categoryRepository.UpdateAsync(category);
            var updatedCategory = await _categoryRepository.GetByIdAsync(id);
            return MapToResponseDto(updatedCategory!);
        }
        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                throw new InvalidOperationException("Categoría no encontrada");
            await _categoryRepository.DeleteAsync(id);
        }
        private CategoryResponseDto MapToResponseDto(Category category)
        {
            return new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                Color = category.Color,
                TasksCount = category.Tasks?.Count ?? 0
            };
        }
    }
}