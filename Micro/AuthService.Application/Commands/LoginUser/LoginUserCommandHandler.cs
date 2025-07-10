using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Commands.LoginUser
{
    public class LoginUserCommandHandler:IRequestHandler<LoginUserCommand,AuthResultDto>
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public LoginUserCommandHandler(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }


        public async Task<AuthResultDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            // 1) E-posta ile kullanıcıya eriş
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser == null)
            {
                return new AuthResultDto
                {
                    Success = false,
                    Errors = new[] { "Geçersiz email veya parola." }
                };
            }

            // 2) Şifre doğrulama
            var passwordCheck = await _userManager.CheckPasswordAsync(existingUser, request.Password);
            if (!passwordCheck)
            {
                return new AuthResultDto
                {
                    Success = false,
                    Errors = new[] { "Geçersiz email veya parola." }
                };
            }

            // 3) JWT token üret
            var token = _jwtTokenGenerator.GenerateToken(existingUser);

            return new AuthResultDto
            {
                Success = true,
                Token = token
            };
        }
    }
}
