using DP_BurLida.Api.Dtos;
using DP_BurLida.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DP_BurLida.Api.Controllers
{
    [ApiController]
    [Route("api/notifications")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NotificationsController : ControllerBase
    {
        private readonly ByrlidaContext _context;

        public NotificationsController(ByrlidaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<NotificationResponse>>> List([FromQuery] bool includeRead = true, [FromQuery] int take = 100)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrWhiteSpace(email))
            {
                return Unauthorized();
            }

            if (take < 1) take = 1;
            if (take > 500) take = 500;

            var query = _context.NotificationModelData
                .AsNoTracking()
                .Where(n => n.RecipientEmail == email);

            if (!includeRead)
            {
                query = query.Where(n => !n.IsRead);
            }

            var items = await query
                .OrderByDescending(n => n.CreatedAt)
                .Take(take)
                .Select(n => new NotificationResponse(
                    n.Id,
                    n.OrderId,
                    n.Message,
                    n.IsRead,
                    n.CreatedAt
                ))
                .ToListAsync();

            return Ok(items);
        }

        [HttpPost("{id:int}/read")]
        public async Task<IActionResult> MarkRead([FromRoute] int id)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrWhiteSpace(email))
            {
                return Unauthorized();
            }

            var notification = await _context.NotificationModelData
                .FirstOrDefaultAsync(n => n.Id == id && n.RecipientEmail == email);

            if (notification == null)
            {
                return NotFound();
            }

            if (!notification.IsRead)
            {
                notification.IsRead = true;
                await _context.SaveChangesAsync();
            }

            return NoContent();
        }

        [HttpPost("read-all")]
        public async Task<IActionResult> MarkAllRead()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrWhiteSpace(email))
            {
                return Unauthorized();
            }

            var unread = await _context.NotificationModelData
                .Where(n => n.RecipientEmail == email && !n.IsRead)
                .ToListAsync();

            if (unread.Count == 0)
            {
                return NoContent();
            }

            foreach (var n in unread)
            {
                n.IsRead = true;
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

