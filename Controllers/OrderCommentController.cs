using DP_BurLida.Data;
using DP_BurLida.Data.ModelsData;
using DP_BurLida.Models;
using DP_BurLida.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DP_BurLida.Controllers
{
    [Authorize]
    public class OrderCommentController : Controller
    {
        private readonly ByrlidaContext _context;
        private readonly IOrderServices _orderService;
        private readonly IBrigadeServices _brigadeService;
        private readonly IUserServices _userService;
        private readonly INotificationService _notificationService;

        public OrderCommentController(
            ByrlidaContext context,
            IOrderServices orderService,
            IBrigadeServices brigadeService,
            IUserServices userService,
            INotificationService notificationService)
        {
            _context = context;
            _orderService = orderService;
            _brigadeService = brigadeService;
            _userService = userService;
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> ListPartial(int orderId, string? returnUrl = null)
        {
            if (orderId <= 0)
                return BadRequest();

            var order = await _orderService.GetByIdAsync(orderId);
            if (!await CanAccessOrder(order))
                return Forbid();

            var comments = await _context.OrderCommentModelData
                .Where(c => c.OrderId == orderId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            var vm = new OrderCommentsPartialViewModel
            {
                OrderId = orderId,
                ReturnUrl = returnUrl,
                Comments = comments
            };

            return PartialView("_OrderCommentsPartial", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int orderId, string text, string? returnUrl = null)
        {
            if (orderId <= 0)
                return BadRequest();

            if (string.IsNullOrWhiteSpace(text))
                return RedirectBackOrDefault(returnUrl, orderId);

            var order = await _orderService.GetByIdAsync(orderId);
            if (!await CanAccessOrder(order))
                return Forbid();

            var createdBy = await GetCurrentUserDisplayName();
            var comment = new OrderCommentModelData
            {
                OrderId = orderId,
                Text = text.Trim(),
                CreatedBy = createdBy,
                CreatedAt = DateTime.Now
            };

            _context.OrderCommentModelData.Add(comment);
            await _context.SaveChangesAsync();

            var orderAddress = string.IsNullOrWhiteSpace(order.City)
                ? $"заявка №{orderId}"
                : order.City.Trim();
            await _notificationService.NotifyOrderCommentAddedAsync(orderId, orderAddress, createdBy, text.Trim());

            return RedirectBackOrDefault(returnUrl, orderId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleDone(int id, int orderId, string? returnUrl = null)
        {
            var comment = await _context.OrderCommentModelData.FindAsync(id);
            if (comment == null || comment.OrderId != orderId)
                return NotFound();

            var order = await _orderService.GetByIdAsync(orderId);
            if (!await CanAccessOrder(order))
                return Forbid();

            comment.IsDone = !comment.IsDone;
            comment.DoneAt = comment.IsDone ? DateTime.Now : null;
            await _context.SaveChangesAsync();

            var orderForAddr = await _orderService.GetByIdAsync(orderId);
            var orderAddress = orderForAddr == null || string.IsNullOrWhiteSpace(orderForAddr.City)
                ? $"заявка №{orderId}"
                : orderForAddr.City.Trim();
            var actor = await GetCurrentUserDisplayName();
            await _notificationService.NotifyOrderCommentToggleDoneAsync(orderId, orderAddress, actor, comment.IsDone);

            return RedirectBackOrDefault(returnUrl, orderId);
        }

        private IActionResult RedirectBackOrDefault(string? returnUrl, int orderId)
        {
            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Details", "Order", new { id = orderId });
        }

        private async Task<string?> GetCurrentUserDisplayName()
        {
            var user = await GetCurrentUserProfile();
            return user?.FullName ?? User.Identity?.Name;
        }

        private async Task<UserModelData?> GetCurrentUserProfile()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (!string.IsNullOrWhiteSpace(email))
            {
                var users = await _userService.GetAllAsync();
                return users.FirstOrDefault(u => u.Email == email);
            }
            return null;
        }

        private async Task<List<OrderModelData>> FilterOrdersForCurrentUser(List<OrderModelData> orders)
        {
            var currentUser = await GetCurrentUserProfile();
            if (currentUser == null || string.IsNullOrWhiteSpace(currentUser.Role))
                return orders;

            if (currentUser.Role != "DrillingMaster" && currentUser.Role != "MountingMaster")
                return orders;

            var brigades = await _brigadeService.GetAllAsync();
            var userBrigadeIds = brigades
                .Where(b => b.ResponsibleUserId == currentUser.Id)
                .Select(b => b.Id)
                .ToList();

            if (!userBrigadeIds.Any())
                return new List<OrderModelData>();

            if (currentUser.Role == "DrillingMaster")
            {
                return orders
                    .Where(o => o.DrillingBrigadeId.HasValue && userBrigadeIds.Contains(o.DrillingBrigadeId.Value))
                    .ToList();
            }

            return orders
                .Where(o => o.ArrangementBrigadeId.HasValue && userBrigadeIds.Contains(o.ArrangementBrigadeId.Value))
                .ToList();
        }

        private async Task<bool> CanAccessOrder(OrderModelData order)
        {
            var filtered = await FilterOrdersForCurrentUser(new List<OrderModelData> { order });
            return filtered.Any();
        }
    }
}

