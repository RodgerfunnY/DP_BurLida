using DP_BurLida.Api.Dtos;
using DP_BurLida.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DP_BurLida.Api.Controllers
{
    [ApiController]
    [Route("api/profile")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProfileController : ControllerBase
    {
        private readonly IUserServices _userServices;

        public ProfileController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpGet]
        public async Task<ActionResult<ProfileResponse>> Get()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrWhiteSpace(email))
            {
                return Unauthorized();
            }

            var allUsers = await _userServices.GetAllAsync();
            var user = allUsers.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(new ProfileResponse(
                Id: user.Id,
                Email: user.Email ?? string.Empty,
                FullName: user.FullName ?? string.Empty,
                Role: user.Role ?? string.Empty,
                IsApproved: user.IsApproved,
                Phone: user.Phone ?? string.Empty
            ));
        }
    }
}

