using DP_BurLida.Data.ModelsData;
using DP_BurLida.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DP_BurLida.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IUserServices _userService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(IUserServices userService, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userService = userService;
            _userManager = userManager;
            _signInManager = signInManager;
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
        public async Task<IActionResult> Register(string email, string password, string confirmPassword, string name, string surname, string phone)
        {
            if (ModelState.IsValid)
            {
                if (password != confirmPassword)
                {
                    ModelState.AddModelError("", "Пароли не совпадают");
                    return View();
                }

                var user = new IdentityUser { UserName = email, Email = email };
                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    // Создаем запись в UserModelData
                    var userModel = new UserModelData
                    {
                        Name = name,
                        Surname = surname,
                        Email = email,
                        Phone = phone
                    };
                    await _userService.CreateAsync(userModel);

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View();
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
                // Простая демонстрационная аутентификация
                // В реальном проекте здесь должна быть проверка пароля
                if (email == "admin@example.com" && password == "admin123")
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, "Администратор"),
                        new Claim(ClaimTypes.Email, email),
                        new Claim(ClaimTypes.NameIdentifier, "1")
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = rememberMe
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
                        new ClaimsPrincipal(claimsIdentity), authProperties);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Неверный email или пароль");
                }
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
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}