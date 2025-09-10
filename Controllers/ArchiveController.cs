using DP_BurLida.Data.ModelsData;
using DP_BurLida.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DP_BurLida.Controllers
{
    public class ArchiveController : Controller
    {
        private readonly IOrderServices _orderService;

        public ArchiveController(IOrderServices orderService)
        {
            _orderService = orderService;
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
            
            return View(orders);
        }

        public async Task<ActionResult> Details(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null || order.Status != "Завершен")
            {
                return NotFound();
            }
            return View(order);
        }
    }
}
