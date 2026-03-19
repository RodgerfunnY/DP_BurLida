using DP_BurLida.Data;
using DP_BurLida.Services.CRUDServics;
using DP_BurLida.Services.Implementations;
using DP_BurLida.Services.Interfaces;
using DP_BurLida.Services.InterfacesServics;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;

namespace DP_BurLida
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            // Одна база данных для доменной модели и Identity
            var defaultConnection = builder.Configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(defaultConnection))
            {
                throw new InvalidOperationException("ConnectionStrings:DefaultConnection is empty or not configured.");
            }

            // Cloud Run env values / UI sometimes wrap the whole value in quotes,
            // or leave a trailing comma, or both: "\"Data Source=...;True\","
            // That breaks SqlConnectionStringBuilder. Normalize before parsing.
            static string NormalizeSqlConnectionString(string? value)
            {
                if (string.IsNullOrWhiteSpace(value)) return value ?? string.Empty;
                var s = value.Trim();

                // Remove trailing commas (common when a JSON/env editor appends ",").
                s = s.TrimEnd();
                while (s.EndsWith(",")) s = s.TrimEnd().TrimEnd(',');

                s = s.Trim();

                // Remove outer quotes repeatedly (e.g. "\"...\"" or "'...').
                while (s.Length >= 2)
                {
                    var first = s[0];
                    var last = s[^1];
                    var isOuterDouble = first == '"' && last == '"';
                    var isOuterSingle = first == '\'' && last == '\'';
                    if (!isOuterDouble && !isOuterSingle) break;
                    s = s.Substring(1, s.Length - 2).Trim();
                }

                // Final cleanup.
                s = s.TrimEnd();
                while (s.EndsWith(",")) s = s.TrimEnd().TrimEnd(',');
                return s.Trim();
            }

            defaultConnection = NormalizeSqlConnectionString(defaultConnection);

            // Validate connection string format early.
            bool isValid = true;
            try
            {
                _ = new SqlConnectionStringBuilder(defaultConnection);
            }
            catch (Exception ex)
            {
                isValid = false;

                // If env-var is broken, try fallback to appsettings.json inside the container.
                try
                {
                    var jsonPath = Path.Combine(builder.Environment.ContentRootPath, "appsettings.json");
                    if (File.Exists(jsonPath))
                    {
                        var jsonConfig = new ConfigurationBuilder()
                            .SetBasePath(builder.Environment.ContentRootPath)
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                            .Build();

                        var fallback = jsonConfig.GetConnectionString("DefaultConnection");
                        fallback = NormalizeSqlConnectionString(fallback);
                        if (!string.IsNullOrWhiteSpace(fallback))
                        {
                            _ = new SqlConnectionStringBuilder(fallback);
                            defaultConnection = fallback;
                            isValid = true;
                        }
                    }
                }
                catch
                {
                    // Ignore fallback errors; we'll throw original below.
                }

                if (!isValid)
                {
                    // Do not leak password to logs.
                    var sanitized = Regex.Replace(defaultConnection, @"(?i)(Password\s*=\s*)[^;]*", "$1***");
                    var startsWithQuote = defaultConnection.TrimStart().StartsWith("\"", StringComparison.Ordinal);
                    var endsWithComma = defaultConnection.TrimEnd().EndsWith(",", StringComparison.Ordinal);
                    throw new InvalidOperationException(
                        $"Invalid SQL connection string format. StartsWithQuote={startsWithQuote};EndsWithComma={endsWithComma};Sanitized='{sanitized}'",
                        ex
                    );
                }
            }
            builder.Services.AddDbContext<ByrlidaContext>(options => options.UseSqlServer(defaultConnection));
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(defaultConnection));
            builder.Services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                // Упрощаем требования к паролю, чтобы допустить admin123
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();
            
            // Auth:
            // - Cookies остаются для MVC (ПК/браузер).
            // - JWT используется для /api (мобильное приложение).
            builder.Services
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.LogoutPath = "/Account/Logout";
                    options.AccessDeniedPath = "/Account/Login";
                    options.ExpireTimeSpan = TimeSpan.FromDays(7);
                    options.SlidingExpiration = true;
                })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    var jwtKey = builder.Configuration["Jwt:Key"];
                    var jwtIssuer = builder.Configuration["Jwt:Issuer"];
                    var jwtAudience = builder.Configuration["Jwt:Audience"];

                    if (string.IsNullOrWhiteSpace(jwtKey))
                    {
                        throw new InvalidOperationException("JWT key is not configured. Set configuration value 'Jwt:Key'.");
                    }

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                        ValidateIssuer = !string.IsNullOrWhiteSpace(jwtIssuer),
                        ValidIssuer = jwtIssuer,
                        ValidateAudience = !string.IsNullOrWhiteSpace(jwtAudience),
                        ValidAudience = jwtAudience,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(2)
                    };
                });

            builder.Services.AddAuthorization();

            builder.Services.AddScoped<Services.Interfaces.ITokenService, Services.Implementations.TokenService>();
            builder.Services.AddScoped<Services.Interfaces.IPushNotificationService, Services.Implementations.FcmPushNotificationService>();
            builder.Services.AddScoped<Services.Interfaces.INotificationService, Services.Implementations.NotificationService>();
            builder.Services.AddScoped<IOrderServices, OrderServices>();
            builder.Services.AddScoped<IUserServices, UserServices>();
            builder.Services.AddScoped<IBrigadeServices, BrigadeServices>();
            builder.Services.AddScoped<ISkladServices, SkladServices>();
            builder.Services.AddScoped(typeof(ICrudServices<>), typeof(CrudServices<>));
            var app = builder.Build();

            // Автоматическое создание администратора при первом запуске
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
                    var userService = services.GetRequiredService<IUserServices>();

                    var adminEmail = "admin@example.com";
                    var adminPassword = "admin123";

                    var identityUser = userManager.FindByEmailAsync(adminEmail).GetAwaiter().GetResult();
                    if (identityUser == null)
                    {
                        identityUser = new IdentityUser
                        {
                            UserName = adminEmail,
                            Email = adminEmail,
                            EmailConfirmed = true
                        };

                        var createResult = userManager.CreateAsync(identityUser, adminPassword).GetAwaiter().GetResult();
                        // Если не получилось создать – просто не трогаем дальше
                    }
                    else
                    {
                        // Пользователь уже есть – убедимся, что у него пароль admin123
                        var hasPassword = userManager.CheckPasswordAsync(identityUser, adminPassword).GetAwaiter().GetResult();
                        if (!hasPassword)
                        {
                            var resetToken = userManager.GeneratePasswordResetTokenAsync(identityUser).GetAwaiter().GetResult();
                            var resetResult = userManager.ResetPasswordAsync(identityUser, resetToken, adminPassword).GetAwaiter().GetResult();
                            // Ошибки игнорируем, чтобы не ломать запуск
                        }
                    }

                    var allUsers = userService.GetAllAsync().GetAwaiter().GetResult();
                    var adminProfile = allUsers.FirstOrDefault(u => u.Email == adminEmail);
                    if (adminProfile == null)
                    {
                        adminProfile = new Data.ModelsData.UserModelData
                        {
                            Name = "Администратор",
                            Surname = string.Empty,
                            Email = adminEmail,
                            Phone = string.Empty,
                            Role = "Admin",
                            IsApproved = true
                        };
                        userService.CreateAsync(adminProfile).GetAwaiter().GetResult();
                    }
                    else
                    {
                        adminProfile.Role = "Admin";
                        adminProfile.IsApproved = true;
                        userService.UpdateAsync(adminProfile).GetAwaiter().GetResult();
                    }
                }
                catch
                {
                    // Игнорируем ошибки и не мешаем запуску приложения
                }
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();
            app.MapRazorPages()
               .WithStaticAssets();

            app.Run();
        }
    }
}