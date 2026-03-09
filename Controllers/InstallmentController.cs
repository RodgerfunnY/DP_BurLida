using DP_BurLida.Data;
using DP_BurLida.Data.ModelsData;
using DP_BurLida.Models;
using DP_BurLida.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DP_BurLida.Controllers
{
    [Authorize(Roles = "Admin,Director")]
    public class InstallmentController : Controller
    {
        private readonly IOrderServices _orderService;
        private readonly ByrlidaContext _context;

        public InstallmentController(IOrderServices orderService, ByrlidaContext context)
        {
            _orderService = orderService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var allOrders = await _orderService.GetAllAsync();

            var installmentOrders = allOrders
                .Where(o => o.IsDrillingInstallment || o.IsArrangementInstallment)
                .ToList();

            var viewModel = new InstallmentListViewModel();

            var orderIds = installmentOrders.Select(o => o.Id).ToList();
            var statuses = await _context.OrderInstallmentPaymentStatus
                .Where(s => orderIds.Contains(s.OrderId))
                .ToListAsync();
            var statusesByOrder = statuses
                .GroupBy(s => s.OrderId)
                .ToDictionary(g => g.Key, g => g.ToDictionary(x => x.SlotKey, x => x));

            foreach (var o in installmentOrders)
            {
                var orderStatuses = statusesByOrder.TryGetValue(o.Id, out var dict)
                    ? dict
                    : new Dictionary<string, OrderInstallmentPaymentStatus>();
                var payments = GetPlannedPayments(o, orderStatuses);

                var plannedSum = payments.Sum(p => p.Amount);
                var paidSum = payments.Where(p => p.IsPaid).Sum(p => p.Amount);
                var remaining = plannedSum - paidSum;

                var nextPayment = payments
                    .Where(p => !p.IsPaid && p.Amount > 0)
                    .OrderBy(p => p.DueDate ?? DateTime.MaxValue)
                    .FirstOrDefault();

                viewModel.Items.Add(new InstallmentRowViewModel
                {
                    OrderId = o.Id,
                    Address = o.City ?? string.Empty,
                    Phone = o.Phone,
                    ClientName = o.NameClient,
                    TotalRemainingAmount = remaining,
                    CurrentPaymentAmount = nextPayment?.Amount ?? 0,
                    CurrentPaymentDueDate = nextPayment?.DueDate,
                    InstallmentEripNumber = o.InstallmentEripNumber,
                    CurrentPaymentSlotKey = nextPayment?.SlotKey,
                    Order = o
                });
            }

            return View(viewModel);
        }

        private static List<InstallmentPaymentInfo> GetPlannedPayments(
            OrderModelData o,
            IReadOnlyDictionary<string, OrderInstallmentPaymentStatus> statuses)
        {
            var list = new List<InstallmentPaymentInfo>();

            void Add(decimal? amount, DateTime? dueDate, string slotKey)
            {
                if (amount.HasValue && amount.Value > 0)
                {
                    statuses.TryGetValue(slotKey, out var st);
                    list.Add(new InstallmentPaymentInfo
                    {
                        Amount = amount.Value,
                        DueDate = dueDate,
                        SlotKey = slotKey,
                        IsPaid = st?.IsPaid ?? false,
                        PaidAt = st?.PaidAt
                    });
                }
            }

            Add(o.DrillingFirstPayment, o.DrillingFirstPaymentDueDate, "Drilling1");
            Add(o.DrillingSecondPayment, o.DrillingSecondPaymentDueDate, "Drilling2");
            Add(o.DrillingThirdPayment, o.DrillingThirdPaymentDueDate, "Drilling3");
            Add(o.DrillingFourthPayment, o.DrillingFourthPaymentDueDate, "Drilling4");

            Add(o.ArrangementFirstPayment, o.ArrangementFirstPaymentDueDate, "Arrangement1");
            Add(o.ArrangementSecondPayment, o.ArrangementSecondPaymentDueDate, "Arrangement2");
            Add(o.ArrangementThirdPayment, o.ArrangementThirdPaymentDueDate, "Arrangement3");
            Add(o.ArrangementFourthPayment, o.ArrangementFourthPaymentDueDate, "Arrangement4");

            return list;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkCurrentPaid(int orderId, string slotKey)
        {
            if (orderId <= 0 || string.IsNullOrWhiteSpace(slotKey))
            {
                return RedirectToAction(nameof(Index));
            }

            var order = await _orderService.GetByIdAsync(orderId);
            if (order == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var status = await _context.OrderInstallmentPaymentStatus
                .FirstOrDefaultAsync(s => s.OrderId == orderId && s.SlotKey == slotKey);

            if (status == null)
            {
                status = new OrderInstallmentPaymentStatus
                {
                    OrderId = orderId,
                    SlotKey = slotKey,
                    IsPaid = true,
                    PaidAt = DateTime.Now
                };
                _context.OrderInstallmentPaymentStatus.Add(status);
            }
            else if (!status.IsPaid)
            {
                status.IsPaid = true;
                status.PaidAt = DateTime.Now;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }

    internal class InstallmentPaymentInfo
    {
        public decimal Amount { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsPaid { get; set; }
        public string SlotKey { get; set; } = string.Empty;
        public DateTime? PaidAt { get; set; }
    }
}

