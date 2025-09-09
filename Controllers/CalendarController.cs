using DP_BurLida.Models;
using DP_BurLida.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace DP_BurLida.Controllers
{
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

            // Ограничиваем год разумными пределами
            if (targetYear < 2020 || targetYear > 2030)
                targetYear = currentDate.Year;

            // Ограничиваем месяц
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

            // Получаем первый день месяца
            var firstDayOfMonth = new DateTime(year, month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            // Получаем первый день недели для отображения (понедельник)
            var firstDayOfWeek = GetFirstDayOfWeek(firstDayOfMonth);
            var lastDayOfWeek = GetLastDayOfWeek(lastDayOfMonth);

            // Получаем все заказы за период
            var orders = await _orderService.GetAllAsync();
            var ordersInPeriod = orders.Where(o => o.CreationTimeData >= firstDayOfWeek && o.CreationTimeData <= lastDayOfWeek).ToList();

            // Создаем дни календаря
            var currentDate = firstDayOfWeek;
            while (currentDate <= lastDayOfWeek)
            {
                var dayOrders = ordersInPeriod.Where(o => o.CreationTimeData.Date == currentDate.Date).ToList();
                
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
            // Преобразуем воскресенье (0) в 7 для правильного расчета
            if (dayOfWeek == 0) dayOfWeek = 7;
            return date.AddDays(-(dayOfWeek - 1));
        }

        private DateTime GetLastDayOfWeek(DateTime date)
        {
            var dayOfWeek = (int)date.DayOfWeek;
            // Преобразуем воскресенье (0) в 7 для правильного расчета
            if (dayOfWeek == 0) dayOfWeek = 7;
            return date.AddDays(7 - dayOfWeek);
        }

        public async Task<IActionResult> DayDetails(DateTime date)
        {
            var orders = await _orderService.GetAllAsync();
            var dayOrders = orders.Where(o => o.CreationTimeData.Date == date.Date).ToList();
            
            ViewBag.SelectedDate = date;
            return View(dayOrders);
        }
    }
}
