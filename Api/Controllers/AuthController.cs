using DP_BurLida.Api.Dtos;
using DP_BurLida.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DP_BurLida.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserServices _userServices;
        private readonly ITokenService _tokenService;

        public AuthController(
            UserManager<IdentityUser> userManager,
            IUserServices userServices,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _userServices = userServices;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Email and password are required.");
            }

            var identityUser = await _userManager.FindByEmailAsync(request.Email);
            if (identityUser == null)
            {
                return Unauthorized();
            }

            var passwordValid = await _userManager.CheckPasswordAsync(identityUser, request.Password);
            if (!passwordValid)
            {
                return Unauthorized();
            }

            var allUsers = await _userServices.GetAllAsync();
            var userModel = allUsers.FirstOrDefault(u => u.Email == request.Email);
            if (userModel == null)
            {
                return Forbid();
            }

            if (!userModel.IsApproved)
            {
                return Forbid();
            }

            var token = _tokenService.CreateAccessToken(userModel);
            return Ok(new LoginResponse(
                AccessToken: token,
                UserId: userModel.Id,
                Email: userModel.Email ?? string.Empty,
                FullName: userModel.FullName ?? string.Empty,
                Role: userModel.Role ?? string.Empty
            ));
        }
    }
}

