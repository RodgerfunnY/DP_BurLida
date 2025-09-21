using DP_BurLida.Data.ModelsData;
using DP_BurLida.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DP_BurLida.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserServices _userService;

        public UserController(IUserServices userService)
        {
            _userService = userService;
        }

        public async Task<ActionResult> Index()
        {
            var user = await _userService.GetAllAsync();
            return View(user);
        }

        // GET: UserController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            return View("Details", user);
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserModelData model)
        {
            if (!ModelState.IsValid)
            {
                    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        System.Diagnostics.Debug.WriteLine($"Ошибка валидации: {error.ErrorMessage}");
                    }
                    return View(model);
                }

                System.Diagnostics.Debug.WriteLine($"Создание пользователя: {model.Name} {model.Surname}, Email: {model.Email}");

                await _userService.CreateAsync(model);
                return RedirectToAction(nameof(Index));
        }

        // GET: UserController/Edit/5
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound();
            return View("Edit", user);
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UserModelData user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            await _userService.UpdateAsync(user);
            return RedirectToAction(nameof(Index));
        }

        // GET: UserController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound();
            return View(user);
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id, IFormCollection collection)
        {
            await _userService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
