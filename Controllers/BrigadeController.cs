using DP_BurLida.Data.ModelsData;
using DP_BurLida.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace DP_BurLida.Controllers
{
    [Authorize]
    public class BrigadeController : Controller
    {
        private readonly IBrigadeServices _brigadeService;
        private readonly IUserServices _userService;

        public BrigadeController(IBrigadeServices brigadeService, IUserServices userService)
        {
            _brigadeService = brigadeService;
            _userService = userService;
        }

        // Вспомогательный метод для загрузки пользователей
        private async Task<SelectList> GetUsersSelectList(int? selectedUserId = null)
        {
            var users = await _userService.GetAllAsync();
            
            // Отладочная информация
            Console.WriteLine($"Загрузка пользователей для SelectList. Количество: {users.Count()}");
            foreach (var user in users)
            {
                Console.WriteLine($"Пользователь: ID={user.Id}, FullName={user.FullName}");
            }
            
            return new SelectList(users, "Id", "FullName", selectedUserId);
        }

        public async Task<ActionResult> Index()
        {
            var brigade = await _brigadeService.GetAllAsync();
            return View(brigade);
        }

        // GET: OrderController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var brigade = await _brigadeService.GetByIdAsync(id);
            return View(brigade);
        }

        // GET: OrderController/Create
        public async Task<ActionResult> Create()
        {
            ViewBag.Users = await GetUsersSelectList();
            return View();
        }

        // POST: OrderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(BrigadeModelData model)
        {
            // Очищаем ошибки валидации для навигационного свойства
            ModelState.Remove("ResponsibleUser");
            
            if (!ModelState.IsValid)
            {
                ViewBag.Users = await GetUsersSelectList(model.ResponsibleUserId);
                return View(model);
            }

            // Отладочная информация
            Console.WriteLine($"Создание бригады: ResponsibleUserId = {model.ResponsibleUserId}");

            await _brigadeService.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }
        // GET: OrderController/Edit/5
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var brigade = await _brigadeService.GetByIdAsync(id);
            if (brigade == null)
                return NotFound();
            
            ViewBag.Users = await GetUsersSelectList(brigade.ResponsibleUserId);
            return View(brigade);
        }

        // POST: OrderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(BrigadeModelData brigade)
        {
            // Очищаем ошибки валидации для навигационного свойства
            ModelState.Remove("ResponsibleUser");
            
            if (!ModelState.IsValid)
            {
                ViewBag.Users = await GetUsersSelectList(brigade.ResponsibleUserId);
                return View(brigade);
            }

            // Отладочная информация
            Console.WriteLine($"Редактирование бригады: ResponsibleUserId = {brigade.ResponsibleUserId}");

            await _brigadeService.UpdateAsync(brigade);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            var brigade = await _brigadeService.GetByIdAsync(id);
            if (brigade == null)
                return NotFound();
            return View(brigade);
        }

        // POST: OrderController/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await _brigadeService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
