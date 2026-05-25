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
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task CreateAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
