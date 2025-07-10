using AuthService.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Commands.LoginUser
{
    public class LoginUserCommand: IRequest<AuthResultDto>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
