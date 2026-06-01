using TodoApp.BLL.DTOs.Categories;

namespace TodoApp.BLL.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync(Guid userId);
        Task CreateAsync(Guid userId, CreateCategoryDto dto);
        Task DeleteAsync(Guid id);
    }
}