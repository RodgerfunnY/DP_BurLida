using DP_BurLida.Data.ModelsData;
using DP_BurLida.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DP_BurLida.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderServices _orderService;
        private readonly IBrigadeServices _brigadeService;

        public OrderController(IOrderServices orderService, IBrigadeServices brigadeService)
        {
            _orderService = orderService;
            _brigadeService = brigadeService;
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
