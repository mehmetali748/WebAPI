using AuthService.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Commands.RegisterUser
{
    public class RegisterUserCommand:IRequest<AuthResultDto>
    {
        public string Email { get; set; }
        public  string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
