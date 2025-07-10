using Microsoft.AspNetCore.Mvc;
using AuthService.Application.Commands.RegisterUser;
using AuthService.Application.Commands.LoginUser;
using AuthService.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;

using System.Threading.Tasks;
namespace AuthService.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST: api/Account/Register
        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok(result);
        }


        // GET: api/Account/Profile
        [HttpGet("Profile")]
        [Authorize]
        public IActionResult GetProfile()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var username = User.Identity.Name;

            return Ok(new
            {
                UserId = userId,
                Email = email,
                UserName = username
            });
        }
    }
}
