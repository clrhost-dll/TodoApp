using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using TodoApp.BLL.DTOs.Tasks;

namespace TodoApp.BLL.Validators.Tasks
{
    public class UpdateTaskValidator
    : AbstractValidator<UpdateTaskDto>
    {
        public UpdateTaskValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(x => x.Description)
                .MaximumLength(1000);
        }
    }
}
