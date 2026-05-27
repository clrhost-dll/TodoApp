using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TodoApp.BLL.DTOs.Tasks;
using TodoApp.BLL.Interfaces;
using TodoApp.BLL.Models;
using TodoApp.DAL.Entities;
using TodoApp.DAL.Repositories.Interfaces;
using TodoApp.BLL.Exceptions;

namespace TodoApp.BLL.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<PagedResult<TaskDto>> GetAllAsync(
            Guid userId,
            TaskQueryDto query)
        {
            IQueryable<TaskItem> tasks =
                _taskRepository.GetQueryable(userId);

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                tasks = tasks.Where(x =>
                    x.Title.Contains(query.Search));
            }

            if (query.CategoryId.HasValue)
            {
                tasks = tasks.Where(x =>
                    x.CategoryId == query.CategoryId.Value);
            }

            int totalCount = await tasks.CountAsync();

            List<TaskDto> items = await tasks
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(x => new TaskDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    IsCompleted = x.IsCompleted,
                    CreatedAt = x.CreatedAt,
                    DueDate = x.DueDate,
                    CategoryName = x.Category.Name
                })
                .ToListAsync();

            return new PagedResult<TaskDto>
            {
                Items = items,
                Page = query.Page,
                PageSize = query.PageSize,
                TotalCount = totalCount
            };
        }

        public async Task<TaskDto?> GetByIdAsync(Guid id)
        {
            TaskItem? task =
                await _taskRepository.GetByIdAsync(id);

            if (task is null)
            {
                return null;
            }

            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
                CreatedAt = task.CreatedAt,
                DueDate = task.DueDate,
                CategoryName = task.Category.Name
            };
        }

        public async Task CreateAsync(
            Guid userId,
            CreateTaskDto dto)
        {
            TaskItem task = new()
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Description = dto.Description,
                CategoryId = dto.CategoryId,
                DueDate = dto.DueDate,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            await _taskRepository.CreateAsync(task);

            await _taskRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(
            Guid id,
            UpdateTaskDto dto)
        {
            TaskItem? task =
                await _taskRepository.GetByIdAsync(id);

            if (task is null)
            {
                throw new NotFoundException("Task not found");
            }

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.IsCompleted = dto.IsCompleted;
            task.CategoryId = dto.CategoryId;
            task.DueDate = dto.DueDate;

            _taskRepository.Update(task);

            await _taskRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            TaskItem? task =
                await _taskRepository.GetByIdAsync(id);

            if (task is null)
            {
                throw new NotFoundException("Task not found");
            }

            _taskRepository.Delete(task);

            await _taskRepository.SaveChangesAsync();
        }
    }
}
