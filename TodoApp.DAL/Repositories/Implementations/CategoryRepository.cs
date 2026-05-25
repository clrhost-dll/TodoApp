using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TodoApp.DAL.Context;
using TodoApp.DAL.Entities;
using TodoApp.DAL.Repositories.Interfaces;

namespace TodoApp.DAL.Repositories.Implementations
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllAsync(Guid userId)
        {
            return await _context.Categories
                .Where(x => x.UserId == userId)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(Guid id)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task CreateAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
