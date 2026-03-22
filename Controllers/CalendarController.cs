using DP_BurLida.Models;
using DP_BurLida.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Linq;
using System.Security.Claims;

namespace DP_BurLida.Controllers
{
    [Authorize]
    public class CalendarController : Controller
    {
        private readonly IOrderServices _orderService;
        private readonly IUserServices _userService;
        private readonly IBrigadeServices _brigadeService;

        public CalendarController(IOrderServices orderService, IUserServices userService, IBrigadeServices brigadeService)
        {
            _orderService = orderService;
            _userService = userService;
            _brigadeService = brigadeService;
        }

        public async Task<IActionResult> Index(int? year, int? month, string? type, int? mw)
        {
            var currentDate = DateTime.Now;
            var targetYear = year ?? currentDate.Year;
            var targetMonth = month ?? currentDate.Month;
            var calendarType = string.IsNullOrWhiteSpace(type) ? "drilling" : type.ToLowerInvariant();

            if (targetYear < 2020 || targetYear > 2030)
                targetYear = currentDate.Year;

            if (targetMonth < 1 || targetMonth > 12)
                targetMonth = currentDate.Month;

            var calendar = await BuildCalendar(targetYear, targetMonth, calendarType);
            ApplyMobileWeekIndex(calendar, mw);
            return View(calendar);
        }

        private static void ApplyMobileWeekIndex(CalendarViewModel calendar, int? mwFromQuery)
        {
            var days = calendar.Days;
            var weekCount = days.Count / 7;
            if (weekCount < 1)
                weekCount = 1;

            var today = DateTime.Today;
            var defaultWeek = 0;
            var foundToday = false;
            for (var w = 0; w < weekCount; w++)
            {
                for (var d = 0; d < 7; d++)
                {
                    var idx = w * 7 + d;
                    if (idx >= days.Count)
                        break;
                    if (days[idx].Date.Date == today)
                    {
                        defaultWeek = w;
                        foundToday = true;
                        break;
                    }
                }

                if (foundToday)
                    break;
            }

            if (!foundToday)
            {
                var firstCurrent = days.FindIndex(x => x.IsCurrentMonth);
                if (firstCurrent >= 0)
                    defaultWeek = firstCurrent / 7;
            }

            var mobileWeek = mwFromQuery ?? defaultWeek;
            if (mobileWeek < 0)
                mobileWeek = 0;
            if (mobileWeek >= weekCount)
                mobileWeek = weekCount - 1;

            calendar.WeekCount = weekCount;
            calendar.MobileWeekIndex = mobileWeek;
        }

        private async Task<CalendarViewModel> BuildCalendar(int year, int month, string calendarType)
        {
            var calendar = new CalendarViewModel
            {
                Year = year,
                Month = month,
                CalendarType = calendarType
            };

            var firstDayOfMonth = new DateTime(year, month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            var firstDayOfWeek = GetFirstDayOfWeek(firstDayOfMonth);
            var lastDayOfWeek = GetLastDayOfWeek(lastDayOfMonth);

            var allOrders = await _orderService.GetAllAsync();

            // Ограничиваем список заявок для мастеров по их бригаде
            var visibleOrders = await FilterOrdersForCurrentUser(allOrders);

            // Фильтрация заказов по типу календаря и установка заголовка
            var filteredOrders = FilterOrdersByType(visibleOrders, calendarType, calendar);

            // Для разных типов календаря используем разные поля даты:
            // drilling/contractors — WorkDate, montage — ArrangementDate.
            bool useArrangementDate = string.Equals(calendarType, "montage", StringComparison.OrdinalIgnoreCase);
            Func<Data.ModelsData.OrderModelData, DateTime?> getDate = o => useArrangementDate ? o.ArrangementDate : o.WorkDate;

            // Заявки без выбранной даты (бурения или обустройства) для правой панели
            calendar.UnscheduledOrders = filteredOrders
                .Where(o => !getDate(o).HasValue)
                .ToList();

            var ordersWithDate = filteredOrders.Where(o => getDate(o).HasValue).ToList();
            var ordersInPeriod = ordersWithDate
                .Where(o =>
                {
                    var d = getDate(o)!.Value;
                    return d >= firstDayOfWeek && d <= lastDayOfWeek;
                })
                .ToList();

            var currentDate = firstDayOfWeek;
            while (currentDate <= lastDayOfWeek)
            {
                var dayOrders = ordersInPeriod
                    .Where(o => getDate(o)!.Value.Date == currentDate.Date)
                    .ToList();
                
                calendar.Days.Add(new CalendarDay
                {
                    Day = currentDate.Day,
                    Date = currentDate,
                    IsCurrentMonth = currentDate.Month == month,
                    IsToday = currentDate.Date == DateTime.Today,
                    Orders = dayOrders
                });

                currentDate = currentDate.AddDays(1);
            }

            return calendar;
        }

        /// <summary>
        /// Фильтрует заказы по типу календаря и выставляет человекочитаемый заголовок.
        /// </summary>
        private IEnumerable<Data.ModelsData.OrderModelData> FilterOrdersByType(
            IEnumerable<Data.ModelsData.OrderModelData> orders,
            string? calendarType,
            CalendarViewModel? calendar = null)
        {
            var type = string.IsNullOrWhiteSpace(calendarType) ? "drilling" : calendarType.ToLowerInvariant();

            var notCompleted = orders.Where(o => o.Status != "Завершен");

            return type switch
            {
                "montage" => SetTitleAndFilter(
                    calendar,
                    "Монтажный график",
                    notCompleted.Where(o =>
                        o.Status == "Обустройство" ||
                        o.ArrangementBrigadeId.HasValue)),
                "contractors" => SetTitleAndFilter(
                    calendar,
                    "Календарь подрядчиков",
                    notCompleted.Where(o => o.Status == "Отдали подрядчикам")),
                _ => SetTitleAndFilter(
                    calendar,
                    "График бурения",
                    notCompleted.Where(o => o.Status == "Ожидание" || o.Status == "Бурение"))
            };
        }

        private IEnumerable<Data.ModelsData.OrderModelData> SetTitleAndFilter(
            CalendarViewModel? calendar,
            string title,
            IEnumerable<Data.ModelsData.OrderModelData> query)
        {
            if (calendar != null)
            {
                calendar.CalendarTitle = title;
            }
            return query;
        }

        private DateTime GetFirstDayOfWeek(DateTime date)
        {
            var dayOfWeek = (int)date.DayOfWeek;
            if (dayOfWeek == 0) dayOfWeek = 7;
            return date.AddDays(-(dayOfWeek - 1));
        }

        private DateTime GetLastDayOfWeek(DateTime date)
        {
            var dayOfWeek = (int)date.DayOfWeek;
            if (dayOfWeek == 0) dayOfWeek = 7;
            return date.AddDays(7 - dayOfWeek);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateWorkDate(int orderId, string newDate, string calendarType)
        {
            if (orderId <= 0 || string.IsNullOrWhiteSpace(newDate))
                return BadRequest("Некорректные данные.");

            if (!DateTime.TryParseExact(newDate, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out var parsedDate))
            {
                return BadRequest("Некорректный формат даты.");
            }

            var order = await _orderService.GetByIdAsync(orderId);
            // В зависимости от активного календаря обновляем дату бурения или обустройства
            if (string.Equals(calendarType, "montage", StringComparison.OrdinalIgnoreCase))
            {
                order.ArrangementDate = parsedDate;
            }
            else
            {
                order.WorkDate = parsedDate;
            }
            await _orderService.UpdateAsync(order);

            return Ok(new { success = true });
        }

        public async Task<IActionResult> DayDetails(DateTime? date, string? type)
        {
            if (!date.HasValue)
                return RedirectToAction("Index");

            var targetDate = date.Value.Date;
            var allOrders = await _orderService.GetAllAsync();
            var visibleOrders = await FilterOrdersForCurrentUser(allOrders);
            var filteredOrders = FilterOrdersByType(visibleOrders, type);
            
            bool useArrangementDate = string.Equals(type, "montage", StringComparison.OrdinalIgnoreCase);
            var dayOrders = filteredOrders
                .Where(o =>
                {
                    var d = useArrangementDate ? o.ArrangementDate : o.WorkDate;
                    return d.HasValue && d.Value.Date == targetDate;
                })
                .ToList();

            ViewBag.SelectedDate = targetDate;
            ViewBag.CalendarType = string.IsNullOrWhiteSpace(type) ? "drilling" : type.ToLowerInvariant();
            return View(dayOrders);
        }

        /// <summary>
        /// Ограничивает список заявок для ролей бурового и монтажного мастеров только их бригадами.
        /// Остальным ролям возвращает полный список.
        /// </summary>
        private async Task<IEnumerable<Data.ModelsData.OrderModelData>> FilterOrdersForCurrentUser(IEnumerable<Data.ModelsData.OrderModelData> orders)
        {
            var currentUser = await GetCurrentUserProfile();
            if (currentUser == null || string.IsNullOrWhiteSpace(currentUser.Role))
                return orders;

            if (currentUser.Role != "DrillingMaster" && currentUser.Role != "MountingMaster")
                return orders;

            var brigades = await _brigadeService.GetAllAsync();
            var userBrigadeIds = brigades
                .Where(b => b.ResponsibleUserId == currentUser.Id)
                .Select(b => b.Id)
                .ToList();

            if (!userBrigadeIds.Any())
                return Enumerable.Empty<Data.ModelsData.OrderModelData>();

            if (currentUser.Role == "DrillingMaster")
            {
                return orders
                    .Where(o => o.DrillingBrigadeId.HasValue &&
                                userBrigadeIds.Contains(o.DrillingBrigadeId.Value));
            }

            // MountingMaster
            return orders
                .Where(o => o.ArrangementBrigadeId.HasValue &&
                            userBrigadeIds.Contains(o.ArrangementBrigadeId.Value));
        }

        /// <summary>
        /// Получает профиль текущего пользователя по его email.
        /// </summary>
        private async Task<Data.ModelsData.UserModelData?> GetCurrentUserProfile()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrWhiteSpace(email))
                return null;

            var allUsers = await _userService.GetAllAsync();
            return allUsers.FirstOrDefault(u => u.Email == email);
        }
    }
}
