using TodoApp.BLL.DTOs.Categories;
using TodoApp.BLL.Exceptions;
using TodoApp.BLL.Interfaces;
using TodoApp.DAL.Entities;
using TodoApp.DAL.Repositories.Interfaces;

namespace TodoApp.BLL.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync(Guid userId)
        {
            IEnumerable<Category> categories =
                await _categoryRepository.GetAllAsync(userId);

            return categories.Select(x => new CategoryDto
            {
                Id = x.Id,
                Name = x.Name
            });
        }

        public async Task CreateAsync(Guid userId, CreateCategoryDto dto)
        {
            Category category = new()
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                UserId = userId
            };

            await _categoryRepository.CreateAsync(category);
            await _categoryRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            Category? category =
                await _categoryRepository.GetByIdAsync(id);

            if (category is null)
            {
                throw new NotFoundException("Category not found");
            }

            _categoryRepository.Delete(category);
            await _categoryRepository.SaveChangesAsync();
        }
    }
}