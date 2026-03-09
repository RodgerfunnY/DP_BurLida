using DP_BurLida.Data.ModelsData;
using DP_BurLida.Data;
using DP_BurLida.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;

namespace DP_BurLida.Controllers
{
    [Authorize]
    public class ArchiveController : Controller
    {
        private readonly IOrderServices _orderService;
        private readonly IUserServices _userService;
        private readonly IBrigadeServices _brigadeService;
        private readonly ByrlidaContext _context;

        public ArchiveController(IOrderServices orderService, IUserServices userService, IBrigadeServices brigadeService, ByrlidaContext context)
        {
            _orderService = orderService;
            _userService = userService;
            _brigadeService = brigadeService;
            _context = context;
        }

        public async Task<ActionResult> Index(string searchTerm)
        {
            var allOrders = await _orderService.GetAllAsync();
            
            var completedOrders = allOrders.Where(o => o.Status == "Завершен").ToList();
            
            List<OrderModelData> orders;
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                orders = await _orderService.SearchAsync(searchTerm);
                orders = orders.Where(o => o.Status == "Завершен").ToList();
                ViewBag.SearchTerm = searchTerm;
            }
            else
            {
                orders = completedOrders;
            }

            // Ограничиваем видимость заявок для мастеров по их бригаде
            orders = await FilterOrdersForCurrentUser(orders);

            return View(orders);
        }

        public async Task<ActionResult> Details(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null || order.Status != "Завершен")
            {
                return NotFound();
            }

            if (!await CanAccessOrder(order))
            {
                return Forbid();
            }

            ViewBag.OrderComments = await _context.OrderCommentModelData
                .Where(c => c.OrderId == id)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            return View(order);
        }

        /// <summary>
        /// Ограничивает список заявок для ролей бурового и монтажного мастеров только их бригадами.
        /// Остальным ролям возвращает полный список.
        /// </summary>
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
                orders = orders
                    .Where(o => o.DrillingBrigadeId.HasValue &&
                                userBrigadeIds.Contains(o.DrillingBrigadeId.Value))
                    .ToList();
            }
            else // MountingMaster
            {
                orders = orders
                    .Where(o => o.ArrangementBrigadeId.HasValue &&
                                userBrigadeIds.Contains(o.ArrangementBrigadeId.Value))
                    .ToList();
            }

            return orders;
        }

        private async Task<bool> CanAccessOrder(OrderModelData order)
        {
            var filtered = await FilterOrdersForCurrentUser(new List<OrderModelData> { order });
            return filtered.Any();
        }

        /// <summary>
        /// Получает профиль текущего пользователя по его email.
        /// </summary>
        private async Task<UserModelData?> GetCurrentUserProfile()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrWhiteSpace(email))
                return null;

            var allUsers = await _userService.GetAllAsync();
            return allUsers.FirstOrDefault(u => u.Email == email);
        }
    }
}
