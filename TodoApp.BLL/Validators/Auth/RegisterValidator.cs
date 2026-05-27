using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using TodoApp.BLL.DTOs.Auth;

namespace TodoApp.BLL.Validators.Auth
{
    public class RegisterValidator
    : AbstractValidator<RegisterDto>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(6);
        }
    }
}
