using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.BLL.DTOs.Tasks;
using TodoApp.BLL.Models;

namespace TodoApp.BLL.Interfaces
{
    public interface ITaskService
    {
        Task<PagedResult<TaskDto>> GetAllAsync(Guid userId,TaskQueryDto query);

        Task<TaskDto?> GetByIdAsync(Guid id);

        Task CreateAsync(Guid userId,CreateTaskDto dto);

        Task UpdateAsync(Guid id,UpdateTaskDto dto);

        Task DeleteAsync(Guid id);
    }
}
