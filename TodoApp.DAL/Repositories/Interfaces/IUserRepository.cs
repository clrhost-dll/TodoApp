using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.DAL.Entities;

namespace TodoApp.DAL.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);

        Task CreateAsync(User user);

        Task SaveChangesAsync();
    }
}
