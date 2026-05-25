using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.BLL.DTOs.Auth
{
    public class RegisterDto
    {
        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
    public class LoginDto
    {
        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
    public class AuthResponseDto
    {
        public string Token { get; set; } = null!;

        public string UserName { get; set; } = null!;
    }
}
