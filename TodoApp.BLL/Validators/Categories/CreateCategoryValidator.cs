using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using TodoApp.BLL.DTOs.Categories;

namespace TodoApp.BLL.Validators.Categories
{
    public class CreateCategoryValidator
    : AbstractValidator<CreateCategoryDto>
    {
        public CreateCategoryValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);
        }
    }
}
