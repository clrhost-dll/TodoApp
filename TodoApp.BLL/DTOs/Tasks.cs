using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.BLL.DTOs.Tasks
{
    public class CreateTaskDto
    {
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public Guid CategoryId { get; set; }

        public DateTime? DueDate { get; set; }
    }
    public class UpdateTaskDto
    {
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public bool IsCompleted { get; set; }

        public Guid CategoryId { get; set; }

        public DateTime? DueDate { get; set; }
    }
    public class TaskDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? DueDate { get; set; }

        public string CategoryName { get; set; } = null!;
    }
    public class TaskQueryDto
    {
        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string? Search { get; set; }

        public Guid? CategoryId { get; set; }
    }
}
