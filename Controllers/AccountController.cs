using DP_BurLida.Data.ModelsData;
using DP_BurLida.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DP_BurLida.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserServices _userService;

        public AccountController(IUserServices userService)
        {
            _userService = userService;
        }

        // GET: Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserModelData user, string password, string confirmPassword)
        {
            if (ModelState.IsValid)
            {
                if (password != confirmPassword)
                {
                    ModelState.AddModelError("", "Пароли не совпадают");
                    return View(user);
                }

                // Здесь должна быть логика создания пользователя
                // await _userService.CreateAsync(user, password);

                return RedirectToAction("Login");
            }
            return View(user);
        }

        // GET: Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password, bool rememberMe = false)
        {
            if (ModelState.IsValid)
            {
                // Здесь должна быть логика аутентификации
                // var result = await _signInManager.PasswordSignInAsync(email, password, rememberMe, lockoutOnFailure: false);

                // Временно перенаправляем на главную страницу
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // GET: Account/Profile
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            // Здесь должна быть логика получения текущего пользователя
            // var user = await _userService.GetCurrentUserAsync();

            // Временно создаем тестового пользователя
            var user = new UserModelData
            {
                Id = 1,
                Name = "Иван",
                Surname = "Иванов",
                Email = "ivan@example.com",
                Phone = "+375291234567"
            };

            return View(user);
        }

        // GET: Account/EditProfile
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            // Здесь должна быть логика получения текущего пользователя
            var user = new UserModelData
            {
                Id = 1,
                Name = "Иван",
                Surname = "Иванов",
                Email = "ivan@example.com",
                Phone = "+375291234567"
            };

            return View(user);
        }

        // POST: Account/EditProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(UserModelData user)
        {
            if (ModelState.IsValid)
            {
                await _userService.UpdateAsync(user);
                return RedirectToAction("Profile");
            }
            return View(user);
        }

        // GET: Account/ChangePassword
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        // POST: Account/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword, string confirmNewPassword)
        {
            if (ModelState.IsValid)
            {
                if (newPassword != confirmNewPassword)
                {
                    ModelState.AddModelError("", "Новые пароли не совпадают");
                    return View();
                }

                // Здесь должна быть логика смены пароля
                // var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

                return RedirectToAction("Profile");
            }
            return View();
        }

        // POST: Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // Здесь должна быть логика выхода
            // await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}