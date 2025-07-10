using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;

using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AuthService.Application.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AuthResultDto>
    {


        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public RegisterUserCommandHandler(
            UserManager<ApplicationUser> userManager,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _userManager = userManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<AuthResultDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return new AuthResultDto
                {
                    Success = true,
                    Errors = new[] { "Email zaten kayıtlı." }

                };

            }

            var newUser = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                Firstname = request.FirstName,
                Lastname = request.LastName,
            };

            // 3) Şifreyi kaydet
            var createResult = await _userManager.CreateAsync(newUser, request.Password);
            if (!createResult.Succeeded)
            {
                return new AuthResultDto
                {
                    Success = false,
                    Errors = createResult.Errors.Select(x => x.Description).ToArray()
                };
            }

            // 4) Role ataması (Varsayılan olarak “User” rolünü ekliyoruz)
            await _userManager.AddToRoleAsync(newUser, "User");

            // 5) JWT token üret
            var token = _jwtTokenGenerator.GenerateToken(newUser);

            return new AuthResultDto
            {
                Success = true,
                Token = token
            };
        }

    }
}
