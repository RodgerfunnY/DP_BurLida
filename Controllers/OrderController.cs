using DP_BurLida.Data.ModelsData;
using DP_BurLida.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace DP_BurLida.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderServices _orderService;

        public OrderController(IOrderServices orderService)
        {
            _orderService = orderService;
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
        public ActionResult Create()
        {
            return View("Create");
        }

        // POST: OrderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(OrderModelData model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            await _orderService.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }
        // GET: OrderController/Edit/5
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null)
                return NotFound();
            return View(order);
        }

        // POST: OrderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(OrderModelData order)
        {
            if (!ModelState.IsValid)
            {
                return View(order);
            }

            try
            {
                await _orderService.UpdateAsync(order);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ошибка при сохранении: " + ex.Message);
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
