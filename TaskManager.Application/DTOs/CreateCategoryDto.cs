using System.ComponentModel.DataAnnotations;
namespace TaskManager.Application.DTOs
{
    public class CreateCategoryDto
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;
        [StringLength(200)]
        public string Description { get; set; } = string.Empty;
        [RegularExpression("^#([A-Fa-f0-9]{6})$", ErrorMessage = "El color debe ser un color hsx valido")]
        public string Color { get; set; } = "#3B82F6";
    }
}
