using DP_BurLida.Api.Dtos;
using DP_BurLida.Data;
using DP_BurLida.Data.ModelsData;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DP_BurLida.Api.Controllers
{
    [ApiController]
    [Route("api/push")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PushController : ControllerBase
    {
        private readonly ByrlidaContext _context;

        public PushController(ByrlidaContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDeviceTokenRequest request)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrWhiteSpace(email))
            {
                return Unauthorized();
            }

            if (string.IsNullOrWhiteSpace(request.Token))
            {
                return BadRequest("Token is required.");
            }

            var platform = string.IsNullOrWhiteSpace(request.Platform) ? "android" : request.Platform.Trim().ToLowerInvariant();
            var token = request.Token.Trim();

            var existingByToken = await _context.DeviceTokenModelData.FirstOrDefaultAsync(t => t.Token == token);
            if (existingByToken != null)
            {
                existingByToken.UserEmail = email;
                existingByToken.Platform = platform;
                existingByToken.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();
                return NoContent();
            }

            var row = new DeviceTokenModelData
            {
                UserEmail = email,
                Token = token,
                Platform = platform,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.DeviceTokenModelData.Add(row);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("unregister")]
        public async Task<IActionResult> Unregister([FromBody] RegisterDeviceTokenRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Token))
            {
                return BadRequest("Token is required.");
            }

            var token = request.Token.Trim();
            var row = await _context.DeviceTokenModelData.FirstOrDefaultAsync(t => t.Token == token);
            if (row == null)
            {
                return NoContent();
            }

            _context.DeviceTokenModelData.Remove(row);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

