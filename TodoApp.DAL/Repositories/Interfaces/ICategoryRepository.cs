using TodoApp.DAL.Entities;

namespace TodoApp.DAL.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync(Guid userId);
        Task<Category?> GetByIdAsync(Guid id);
        Task CreateAsync(Category category);
        void Delete(Category category);

        Task SaveChangesAsync();
    }
}