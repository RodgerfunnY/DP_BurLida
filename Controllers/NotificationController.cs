using DP_BurLida.Data;
using DP_BurLida.Data.ModelsData;
using DP_BurLida.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DP_BurLida.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly ByrlidaContext _context;
        private readonly IUserServices _userService;

        public NotificationController(ByrlidaContext context, IUserServices userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var email = GetCurrentUserEmail();
            if (string.IsNullOrWhiteSpace(email))
            {
                return View(new List<NotificationModelData>());
            }

            var notifications = await _context.NotificationModelData
                .Where(n => n.RecipientEmail == email)
                .OrderByDescending(n => n.CreatedAt)
                .Take(100)
                .ToListAsync();

            return View(notifications);
        }

        /// <summary>
        /// Непрочитанные уведомления для всплывающих toast (опрос с клиента).
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ToastUnread()
        {
            var email = GetCurrentUserEmail();
            if (string.IsNullOrWhiteSpace(email))
                return Json(new { items = Array.Empty<object>() });

            var items = await _context.NotificationModelData
                .AsNoTracking()
                .Where(n => n.RecipientEmail == email && !n.IsRead)
                .OrderByDescending(n => n.CreatedAt)
                .Take(30)
                .Select(n => new
                {
                    id = n.Id,
                    message = n.Message,
                    orderId = n.OrderId,
                    createdAt = n.CreatedAt
                })
                .ToListAsync();

            return Json(new { items });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var email = GetCurrentUserEmail();
            if (string.IsNullOrWhiteSpace(email))
                return RedirectToAction(nameof(Index));

            var notification = await _context.NotificationModelData
                .FirstOrDefaultAsync(n => n.Id == id && n.RecipientEmail == email);

            if (notification != null && !notification.IsRead)
            {
                notification.IsRead = true;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var email = GetCurrentUserEmail();
            if (string.IsNullOrWhiteSpace(email))
                return RedirectToAction(nameof(Index));

            var unread = await _context.NotificationModelData
                .Where(n => n.RecipientEmail == email && !n.IsRead)
                .ToListAsync();

            if (unread.Any())
            {
                foreach (var n in unread)
                {
                    n.IsRead = true;
                }
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private string? GetCurrentUserEmail()
        {
            return User.FindFirst(ClaimTypes.Email)?.Value ?? User.Identity?.Name;
        }
    }
}

