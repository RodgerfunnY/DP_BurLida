using DP_BurLida.Data.ModelsData;
using DP_BurLida.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace DP_BurLida.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderServices _orderService;
        private readonly IBrigadeServices _brigadeService;
        private readonly IUserServices _userService;

        public OrderController(IOrderServices orderService, IBrigadeServices brigadeService, IUserServices userService)
        {
            _orderService = orderService;
            _brigadeService = brigadeService;
            _userService = userService;
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
            
            return View(orders);
        }

        // GET: OrderController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            return View(order);
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
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            if (!string.IsNullOrWhiteSpace(email))
            {
                var users = await _userService.GetAllAsync();
                var user = users.FirstOrDefault(u => u.Email == email);
                if (user != null)
                {
                    return user.FullName;
                }
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
            return View(order);
        }

        // POST: OrderController/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await _orderService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
