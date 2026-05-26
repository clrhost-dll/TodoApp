using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.DAL.Entities;

namespace TodoApp.DAL.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        IQueryable<TaskItem> GetQueryable(Guid userId);

        Task<TaskItem?> GetByIdAsync(Guid id);

        Task CreateAsync(TaskItem task);

        void Update(TaskItem task);

        void Delete(TaskItem task);

        Task SaveChangesAsync();
    }
}
