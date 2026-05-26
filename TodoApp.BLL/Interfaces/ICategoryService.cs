using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.BLL.DTOs.Categories;

namespace TodoApp.BLL.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync(
            Guid userId);

        Task CreateAsync(
            Guid userId,
            CreateCategoryDto dto);
    }
}
