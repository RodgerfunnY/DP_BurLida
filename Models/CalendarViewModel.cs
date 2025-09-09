using DP_BurLida.Data.ModelsData;

namespace DP_BurLida.Models
{
    public class CalendarViewModel
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public List<CalendarDay> Days { get; set; } = new List<CalendarDay>();
        public string MonthName => GetMonthName(Month);
        
        private string GetMonthName(int month)
        {
            return month switch
            {
                1 => "Январь",
                2 => "Февраль", 
                3 => "Март",
                4 => "Апрель",
                5 => "Май",
                6 => "Июнь",
                7 => "Июль",
                8 => "Август",
                9 => "Сентябрь",
                10 => "Октябрь",
                11 => "Ноябрь",
                12 => "Декабрь",
                _ => "Неизвестный месяц"
            };
        }
    }

    public class CalendarDay
    {
        public int Day { get; set; }
        public DateTime Date { get; set; }
        public bool IsCurrentMonth { get; set; }
        public bool IsToday { get; set; }
        public List<OrderModelData> Orders { get; set; } = new List<OrderModelData>();
        public bool HasOrders => Orders.Any();
    }
}
