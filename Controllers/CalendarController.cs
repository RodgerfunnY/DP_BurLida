using DP_BurLida.Models;
using DP_BurLida.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace DP_BurLida.Controllers
{
    [Authorize]
    public class CalendarController : Controller
    {
        private readonly IOrderServices _orderService;

        public CalendarController(IOrderServices orderService)
        {
            _orderService = orderService;
        }

        public async Task<IActionResult> Index(int? year, int? month)
        {
            var currentDate = DateTime.Now;
            var targetYear = year ?? currentDate.Year;
            var targetMonth = month ?? currentDate.Month;

            if (targetYear < 2020 || targetYear > 2030)
                targetYear = currentDate.Year;

            if (targetMonth < 1 || targetMonth > 12)
                targetMonth = currentDate.Month;

            var calendar = await BuildCalendar(targetYear, targetMonth);
            return View(calendar);
        }

        private async Task<CalendarViewModel> BuildCalendar(int year, int month)
        {
            var calendar = new CalendarViewModel
            {
                Year = year,
                Month = month
            };

            var firstDayOfMonth = new DateTime(year, month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            var firstDayOfWeek = GetFirstDayOfWeek(firstDayOfMonth);
            var lastDayOfWeek = GetLastDayOfWeek(lastDayOfMonth);

            var orders = await _orderService.GetAllAsync();
            var ordersWithWorkDate = orders.Where(o => o.WorkDate.HasValue).ToList();
            var ordersInPeriod = ordersWithWorkDate.Where(o => o.WorkDate.Value >= firstDayOfWeek && o.WorkDate.Value <= lastDayOfWeek).ToList();

            var currentDate = firstDayOfWeek;
            while (currentDate <= lastDayOfWeek)
            {
                var dayOrders = ordersInPeriod.Where(o => o.WorkDate.Value.Date == currentDate.Date).ToList();
                
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

        public async Task<IActionResult> DayDetails(DateTime? date)
        {
            if (!date.HasValue)
                return RedirectToAction("Index");

            var targetDate = date.Value.Date;
            var orders = await _orderService.GetAllAsync();
            
            var dayOrders = orders.Where(o => o.WorkDate.HasValue && o.WorkDate.Value.Date == targetDate).ToList();

            ViewBag.SelectedDate = targetDate;
            return View(dayOrders);
        }
    }
}
