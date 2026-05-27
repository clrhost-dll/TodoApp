using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using TodoApp.BLL.DTOs.Auth;

namespace TodoApp.BLL.Validators.Auth
{
    public class LoginValidator
    : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty();
        }
    }
}
