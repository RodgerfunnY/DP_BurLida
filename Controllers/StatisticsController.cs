using DP_BurLida.Models;
using DP_BurLida.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DP_BurLida.Controllers
{
    [Authorize(Roles = "Admin,Director")]
    public class StatisticsController : Controller
    {
        private readonly IOrderServices _orderService;
        private readonly IUserServices _userService;
        private readonly IBrigadeServices _brigadeService;

        public StatisticsController(
            IOrderServices orderService,
            IUserServices userService,
            IBrigadeServices brigadeService)
        {
            _orderService = orderService;
            _userService = userService;
            _brigadeService = brigadeService;
        }

        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate)
        {
            var now = DateTime.Today;
            var from = startDate?.Date ?? now.AddMonths(-1);
            var to = endDate?.Date ?? now;

            if (from > to)
            {
                (from, to) = (to, from);
            }

            var orders = await _orderService.GetAllAsync();
            var users = await _userService.GetAllAsync();
            var brigades = await _brigadeService.GetAllAsync();

            var approvedUsers = users.Where(u => u.IsApproved).ToList();

            var managers = approvedUsers
                .Where(u => u.Role == "Manager")
                .ToList();

            var managerStats = managers
                .Select(m =>
                {
                    var ordersCount = orders
                        .Where(o =>
                            !string.IsNullOrEmpty(o.CreatedBy) &&
                            o.CreatedBy == m.FullName &&
                            o.CreationTimeData.Date >= from &&
                            o.CreationTimeData.Date <= to)
                        .Count();

                    return new ManagerStatisticsItem
                    {
                        UserId = m.Id,
                        FullName = m.FullName,
                        Email = m.Email,
                        TotalOrders = ordersCount
                    };
                })
                .OrderByDescending(m => m.TotalOrders)
                .ThenBy(m => m.FullName)
                .ToList();

            var drillingBrigades = brigades
                .Where(b =>
                    b.ResponsibleUserId.HasValue &&
                    approvedUsers.Any(u => u.Id == b.ResponsibleUserId && u.Role == "DrillingMaster"))
                .ToList();

            var drillingStats = drillingBrigades
                .Select(b =>
                {
                    var responsible = approvedUsers.FirstOrDefault(u => u.Id == b.ResponsibleUserId);

                    var totalMeters = orders
                        .Where(o =>
                            o.DrillingBrigadeId == b.Id &&
                            o.WorkDate.HasValue &&
                            o.WorkDate.Value.Date >= from &&
                            o.WorkDate.Value.Date <= to &&
                            o.MetersCount.HasValue)
                        .Sum(o => o.MetersCount ?? 0);

                    return new DrillingBrigadeStatisticsItem
                    {
                        BrigadeId = b.Id,
                        BrigadeName = b.NameBrigade,
                        ResponsibleMasterName = responsible?.FullName,
                        TotalMeters = totalMeters
                    };
                })
                .OrderByDescending(b => b.TotalMeters)
                .ThenBy(b => b.BrigadeName)
                .ToList();

            var mountingMasters = approvedUsers
                .Where(u => u.Role == "MountingMaster")
                .ToList();

            var mountingStats = mountingMasters
                .Select(m =>
                {
                    var masterBrigadeIds = brigades
                        .Where(b => b.ResponsibleUserId == m.Id)
                        .Select(b => b.Id)
                        .ToList();

                    var completedCount = orders
                        .Where(o =>
                            o.Status == "Завершен" &&
                            o.ArrangementBrigadeId.HasValue &&
                            masterBrigadeIds.Contains(o.ArrangementBrigadeId.Value) &&
                            o.ArrangementDate.HasValue &&
                            o.ArrangementDate.Value.Date >= from &&
                            o.ArrangementDate.Value.Date <= to)
                        .Count();

                    return new MountingMasterStatisticsItem
                    {
                        UserId = m.Id,
                        FullName = m.FullName,
                        Email = m.Email,
                        TotalCompletedObjects = completedCount
                    };
                })
                .OrderByDescending(m => m.TotalCompletedObjects)
                .ThenBy(m => m.FullName)
                .ToList();

            var model = new StatisticsViewModel
            {
                StartDate = from,
                EndDate = to,
                Managers = managerStats,
                DrillingBrigades = drillingStats,
                MountingMasters = mountingStats
            };

            return View(model);
        }
    }
}

