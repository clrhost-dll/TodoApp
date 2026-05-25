using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.DAL.Entities;

namespace TodoApp.DAL.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync(Guid userId);

        Task<Category?> GetByIdAsync(Guid id);

        Task CreateAsync(Category category);

        Task SaveChangesAsync();
    }
}
