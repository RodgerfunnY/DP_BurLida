using DP_BurLida.Data.ModelsData;
using DP_BurLida.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DP_BurLida.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderServices _orderService;

        public OrderController(IOrderServices orderService)
        {
            _orderService = orderService;
        }

        public async Task<IActionResult> Index()
        {
            var order = await _orderService.GetAllOrder();
            return View(order);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create (OrderModelData order)
        {
            if (ModelState.IsValid)
            {
                order.CreationTimeData = DateTime.Now; // Устанавливаем текущее время
                await _orderService.AddOrder(order);
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }
        public async Task<IActionResult> Details(int id) // Принимаем только ID
        {
            var order = await _orderService.GetOrderById(id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var order = await _orderService.GetOrderById(id);
            if (order == null)
            {
                return NotFound(); // Если заявка не найдена
            }
            return View(order); // Передаём данные в представление
        }

        [HttpPost]
        public async Task<IActionResult> Edit(OrderModelData order, int id)
        {
            if (!ModelState.IsValid)
            {
                return View(order);
            }

            try
            {
                var existingOrder = await _orderService.GetOrderById(id);
                if (existingOrder == null)
                {
                    return NotFound();
                }

                // Здесь может быть код для обновления полей existingOrder на основе order

                await _orderService.UpdateOrder(existingOrder);
                return RedirectToAction(nameof(Index)); // или другую страницу
            }
            catch (Exception ex)
            {
                // Логирование ошибки
                ModelState.AddModelError("", "Произошла ошибка при обновлении заказа");
                return View(order);
            }
        }

        // Добавьте методы Edit, Delete

    }
}
