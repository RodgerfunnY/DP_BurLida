using System.Diagnostics;
using DP_BurLida.Models;
using DP_BurLida.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DP_BurLida.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IOrderServices _orderService;

        public HomeController(ILogger<HomeController> logger, IOrderServices orderService)
        {
            _logger = logger;
            _orderService = orderService;
        }

        public async Task<IActionResult> Index()
        {
            var allOrders = await _orderService.GetAllAsync();
            
            var statistics = new
            {
                TotalOrders = allOrders.Count,
                WaitingOrders = allOrders.Count(o => o.Status == "Ожидание"),
                InWorkOrders = allOrders.Count(o => o.Status == "В работе"),
                RepairOrders = allOrders.Count(o => o.Status == "Ремонт"),
                CompletedOrders = allOrders.Count(o => o.Status == "Завершен")
            };
            
            ViewBag.Statistics = statistics;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
