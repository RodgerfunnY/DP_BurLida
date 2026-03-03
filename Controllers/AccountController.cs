using DP_BurLida.Data.ModelsData;
using DP_BurLida.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace DP_BurLida.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IUserServices _userService;
        private readonly UserManager<IdentityUser> _userManager;

        public AccountController(IUserServices userService, UserManager<IdentityUser> userManager)
        {
            _userService = userService;
            _userManager = userManager;
        }

        // GET: Account/Register
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
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
                    // Создаем запись в UserModelData c ролью Pending (ожидает подтверждения)
                    var userModel = new UserModelData
                    {
                        Name = name,
                        Surname = surname,
                        Email = email,
                        Phone = phone,
                        Role = "Pending",
                        IsApproved = false
                    };
                    await _userService.CreateAsync(userModel);

                    // После регистрации просим дождаться подтверждения администратора/директора
                    return RedirectToAction("Login");
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
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string email, string password, bool rememberMe = false)
        {
            if (ModelState.IsValid)
            {
                var identityUser = await _userManager.FindByEmailAsync(email);
                if (identityUser == null)
                {
                    ModelState.AddModelError("", "Неверный email или пароль");
                    return View();
                }

                var passwordValid = await _userManager.CheckPasswordAsync(identityUser, password);
                if (!passwordValid)
                {
                    ModelState.AddModelError("", "Неверный email или пароль");
                    return View();
                }

                var allUsers = await _userService.GetAllAsync();
                var userModel = allUsers.FirstOrDefault(u => u.Email == email);

                if (userModel == null)
                {
                    ModelState.AddModelError("", "Профиль сотрудника не найден. Обратитесь к администратору.");
                    return View();
                }

                if (!userModel.IsApproved)
                {
                    ModelState.AddModelError("", "Ваш аккаунт ещё не подтверждён администратором или директором.");
                    return View();
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userModel.FullName),
                    new Claim(ClaimTypes.Email, userModel.Email),
                    new Claim(ClaimTypes.NameIdentifier, userModel.Id.ToString())
                };

                if (!string.IsNullOrWhiteSpace(userModel.Role))
                {
                    claims.Add(new Claim(ClaimTypes.Role, userModel.Role));
                }

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = rememberMe
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // GET: Account/Profile
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var user = await GetCurrentUserProfile();

            if (user == null)
            {
                return RedirectToAction("Login");
            }

            return View(user);
        }

        // GET: Account/EditProfile
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditProfile()
        {
            var user = await GetCurrentUserProfile();

            if (user == null)
            {
                return RedirectToAction("Login");
            }

            return View(user);
        }

        // POST: Account/EditProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> EditProfile(UserModelData user)
        {
            if (ModelState.IsValid)
            {
                var existing = await _userService.GetByIdAsync(user.Id);
                if (existing == null)
                {
                    return NotFound();
                }

                existing.Name = user.Name;
                existing.Surname = user.Surname;
                existing.Phone = user.Phone;

                // Email, роль и подтверждение меняются только через администратора/директора

                await _userService.UpdateAsync(existing);
                return RedirectToAction("Profile");
            }
            return View(user);
        }

        // GET: Account/ChangePassword
        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        // POST: Account/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult ChangePassword(string currentPassword, string newPassword, string confirmNewPassword)
        {
            if (ModelState.IsValid)
            {
                if (newPassword != confirmNewPassword)
                {
                    ModelState.AddModelError("", "Новые пароли не совпадают");
                    return View();
                }

                return RedirectToAction("Profile");
            }
            return View();
        }

        // POST: Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Возвращает профиль текущего пользователя из таблицы сотрудников по email.
        /// </summary>
        private async Task<UserModelData?> GetCurrentUserProfile()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrWhiteSpace(email))
                return null;

            var allUsers = await _userService.GetAllAsync();
            return allUsers.FirstOrDefault(u => u.Email == email);
        }
    }
}