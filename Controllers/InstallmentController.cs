using DP_BurLida.Data.ModelsData;
using DP_BurLida.Models;
using DP_BurLida.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DP_BurLida.Controllers
{
    [Authorize(Roles = "Admin,Director")]
    public class InstallmentController : Controller
    {
        private readonly IOrderServices _orderService;

        public InstallmentController(IOrderServices orderService)
        {
            _orderService = orderService;
        }

        public async Task<IActionResult> Index()
        {
            var allOrders = await _orderService.GetAllAsync();

            var installmentOrders = allOrders
                .Where(o => o.IsDrillingInstallment || o.IsArrangementInstallment)
                .ToList();

            var viewModel = new InstallmentListViewModel();

            foreach (var o in installmentOrders)
            {
                var payments = GetPlannedPayments(o);

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
                    Order = o
                });
            }

            return View(viewModel);
        }

        private static List<InstallmentPaymentInfo> GetPlannedPayments(OrderModelData o)
        {
            var list = new List<InstallmentPaymentInfo>();

            void Add(decimal? amount, DateTime? dueDate)
            {
                if (amount.HasValue && amount.Value > 0)
                {
                    list.Add(new InstallmentPaymentInfo
                    {
                        Amount = amount.Value,
                        DueDate = dueDate,
                        IsPaid = false
                    });
                }
            }

            Add(o.DrillingFirstPayment, o.DrillingFirstPaymentDueDate);
            Add(o.DrillingSecondPayment, o.DrillingSecondPaymentDueDate);
            Add(o.DrillingThirdPayment, o.DrillingThirdPaymentDueDate);
            Add(o.DrillingFourthPayment, o.DrillingFourthPaymentDueDate);

            Add(o.ArrangementFirstPayment, o.ArrangementFirstPaymentDueDate);
            Add(o.ArrangementSecondPayment, o.ArrangementSecondPaymentDueDate);
            Add(o.ArrangementThirdPayment, o.ArrangementThirdPaymentDueDate);
            Add(o.ArrangementFourthPayment, o.ArrangementFourthPaymentDueDate);

            return list;
        }
    }

    internal class InstallmentPaymentInfo
    {
        public decimal Amount { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsPaid { get; set; }
    }
}

