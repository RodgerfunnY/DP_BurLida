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

            // Cloud Run env values / UI forms sometimes wrap the whole value in quotes
            // or leave a trailing comma. Strip only the outer noise to keep it valid.
            defaultConnection = defaultConnection.Trim();
            if (defaultConnection.Length >= 2)
            {
                var first = defaultConnection[0];
                var last = defaultConnection[^1];
                if ((first == '"' && last == '"') || (first == '\'' && last == '\''))
                    defaultConnection = defaultConnection.Substring(1, defaultConnection.Length - 2).Trim();
            }
            defaultConnection = defaultConnection.TrimEnd().TrimEnd(',');

            try
            {
                // Validate connection string format early.
                _ = new SqlConnectionStringBuilder(defaultConnection);
            }
            catch (Exception ex)
            {
                // Do not leak password to logs.
                var sanitized = Regex.Replace(
                    defaultConnection,
                    @"(?i)(Password\s*=\s*)[^;]*",
                    "$1***"
                );
                // Help debug formatting issues (quotes/commas/newlines) without leaking the password.
                var escaped = sanitized
                    .Replace("\r", "\\r")
                    .Replace("\n", "\\n")
                    .Replace("\t", "\\t");

                var startsWithQuote = defaultConnection.TrimStart().StartsWith("\"", StringComparison.Ordinal);
                var endsWithQuote = defaultConnection.TrimEnd().EndsWith("\"", StringComparison.Ordinal);
                var endsWithComma = defaultConnection.TrimEnd().EndsWith(",", StringComparison.Ordinal);

                var lastChunk = escaped.Length > 60 ? escaped.Substring(escaped.Length - 60) : escaped;

                throw new InvalidOperationException(
                    "Invalid SQL connection string format. " +
                    $"StartsWithQuote={startsWithQuote};EndsWithQuote={endsWithQuote};EndsWithComma={endsWithComma};" +
                    $"SanitizedLast60='{lastChunk}'",
                    ex
                );
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