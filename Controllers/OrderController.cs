using DP_BurLida.Data.ModelsData;
using DP_BurLida.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Linq;
using DP_BurLida.Data;

namespace DP_BurLida.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderServices _orderService;
        private readonly IBrigadeServices _brigadeService;
        private readonly IUserServices _userService;
        private readonly ByrlidaContext _context;

        public OrderController(IOrderServices orderService, IBrigadeServices brigadeService, IUserServices userService, ByrlidaContext context)
        {
            _orderService = orderService;
            _brigadeService = brigadeService;
            _userService = userService;
            _context = context;
        }

        private async Task LoadBrigadesForView(int? drillingBrigadeId = null, int? arrangementBrigadeId = null)
        {
            var brigades = await _brigadeService.GetAllAsync();
            ViewBag.DrillingBrigades = new SelectList(brigades, "Id", "NameBrigade", drillingBrigadeId);
            ViewBag.ArrangementBrigades = new SelectList(brigades, "Id", "NameBrigade", arrangementBrigadeId);
        }

        public async Task<ActionResult> Index(string searchTerm)
        {
            List<OrderModelData> orders;
            
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                orders = await _orderService.SearchAsync(searchTerm);
                orders = orders.Where(o => o.Status != "Завершен").ToList();
                ViewBag.SearchTerm = searchTerm;
            }
            else
            {
                var allOrders = await _orderService.GetAllAsync();
                orders = allOrders.Where(o => o.Status != "Завершен").ToList();
            }

            // Ограничиваем видимость заявок для мастеров по их бригаде
            orders = await FilterOrdersForCurrentUser(orders);

            return View(orders);
        }

        // GET: OrderController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null)
                return NotFound();

            if (!await CanAccessOrder(order))
                return Forbid();

            ViewBag.OrderComments = await _context.OrderCommentModelData
                .Where(c => c.OrderId == id)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            return View(order);
        }

        [HttpGet]
        public async Task<IActionResult> QuickDetailsPartial(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null)
                return NotFound();

            if (!await CanAccessOrder(order))
                return Forbid();

            return PartialView("_OrderQuickDetailsPartial", order);
        }

        // GET: OrderController/Create
        public async Task<ActionResult> Create()
        {
            await LoadBrigadesForView();
            return View("Create");
        }

        // POST: OrderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(OrderModelData model)
        {
            ModelState.Remove("DrillingBrigade");
            ModelState.Remove("ArrangementBrigade");
            
            if (!ModelState.IsValid)
            {
                await LoadBrigadesForView(model.DrillingBrigadeId, model.ArrangementBrigadeId);
                return View(model);
            }
            // Поля, которые больше не вводятся вручную в форме
            model.SurnameClient ??= string.Empty;
            model.Area ??= string.Empty;
            model.District ??= string.Empty;
            model.City = model.Address ?? string.Empty;
            // Обустройство в БД не допускает NULL, задаём значение по умолчанию
            model.Arrangement ??= "Не нужно";

            // Автоматически заполняем, кто создал заявку, из личного кабинета/аккаунта
            model.CreatedBy = await GetCurrentUserDisplayName();

            await _orderService.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }
        
        /// <summary>
        /// Получает отображаемое имя текущего пользователя для записи в заявку.
        /// Сначала пытается взять профиль из UserModelData по email, иначе берёт User.Identity.Name.
        /// </summary>
        private async Task<string?> GetCurrentUserDisplayName()
        {
            var user = await GetCurrentUserProfile();
            if (user != null)
            {
                return user.FullName;
            }

            return User.Identity?.Name;
        }
        // GET: OrderController/Edit/5
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null)
                return NotFound();

            if (!await CanAccessOrder(order))
                return Forbid();
            
            order.Address = order.City;
            await LoadBrigadesForView(order.DrillingBrigadeId, order.ArrangementBrigadeId);
            return View(order);
        }

        // POST: OrderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(OrderModelData order)
        {
            ModelState.Remove("DrillingBrigade");
            ModelState.Remove("ArrangementBrigade");

            if (!ModelState.IsValid)
            {
                await LoadBrigadesForView(order.DrillingBrigadeId, order.ArrangementBrigadeId);
                return View(order);
            }

            try
            {
                order.SurnameClient ??= string.Empty;
                order.Area ??= string.Empty;
                order.District ??= string.Empty;
                order.City = order.Address ?? string.Empty;
                order.Arrangement ??= "Не нужно";

                await _orderService.UpdateAsync(order);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ошибка при сохранении: " + ex.Message);
                await LoadBrigadesForView(order.DrillingBrigadeId, order.ArrangementBrigadeId);
                return View(order);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null)
                return NotFound();

            if (!await CanAccessOrder(order))
                return Forbid();
            return View(order);
        }

        // POST: OrderController/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null)
                return NotFound();

            if (!await CanAccessOrder(order))
                return Forbid();

            await _orderService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
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

            if (!string.IsNullOrWhiteSpace(email))
            {
                var users = await _userService.GetAllAsync();
                return users.FirstOrDefault(u => u.Email == email);
            }

            return null;
        }
    }
}
