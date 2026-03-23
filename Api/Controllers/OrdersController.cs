using DP_BurLida.Api.Dtos;
using DP_BurLida.Data.ModelsData;
using DP_BurLida.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DP_BurLida.Api.Controllers
{
    [ApiController]
    [Route("api/orders")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderServices _orderServices;
        private readonly IBrigadeServices _brigadeServices;
        private readonly IUserServices _userServices;
        private readonly INotificationService _notificationService;

        public OrdersController(
            IOrderServices orderServices,
            IBrigadeServices brigadeServices,
            IUserServices userServices,
            INotificationService notificationService)
        {
            _orderServices = orderServices;
            _brigadeServices = brigadeServices;
            _userServices = userServices;
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderListItemResponse>>> List(
            [FromQuery] string? searchTerm = null,
            [FromQuery] bool includeCompleted = false)
        {
            var orders = !string.IsNullOrWhiteSpace(searchTerm)
                ? await _orderServices.SearchAsync(searchTerm)
                : await _orderServices.GetAllAsync();

            if (!includeCompleted)
            {
                orders = orders.Where(o => o.Status != "Завершен").ToList();
            }

            orders = await FilterOrdersForCurrentUser(orders);

            var result = orders
                .OrderByDescending(o => o.CreationTimeData)
                .Select(o => new OrderListItemResponse(
                    o.Id,
                    o.NameClient,
                    o.Phone,
                    o.City,
                    o.Status,
                    o.CreatedBy,
                    o.CreationTimeData,
                    o.WorkDate,
                    o.ArrangementDate
                ))
                .ToList();

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderDetailsResponse>> Get([FromRoute] int id)
        {
            var order = await _orderServices.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            if (!await CanAccessOrder(order))
            {
                return Forbid();
            }

            return Ok(ToDetails(order));
        }

        [HttpPost]
        public async Task<ActionResult<OrderDetailsResponse>> Create([FromBody] OrderUpsertRequest request)
        {
            var currentUser = await GetCurrentUserProfile();
            if (currentUser == null)
            {
                return Unauthorized();
            }

            if (!currentUser.IsApproved)
            {
                return Forbid();
            }

            var model = new OrderModelData
            {
                NameClient = request.NameClient,
                SurnameClient = request.SurnameClient ?? string.Empty,
                Phone = request.Phone,
                Area = request.Area ?? string.Empty,
                District = request.District ?? string.Empty,
                City = request.City ?? string.Empty,
                Diameter = request.Diameter,
                PricePerMeter = request.PricePerMeter,
                Pump = request.Pump,
                MetersCount = request.MetersCount,
                Depth = request.Depth,
                StaticLevel = request.StaticLevel,
                DynamicLevel = request.DynamicLevel,
                Filter = request.Filter,
                PumpModel = request.PumpModel,
                Arrangement = request.Arrangement ?? "Не нужно",
                PumpInstalled = request.PumpInstalled,
                ArrangementDone = request.ArrangementDone,
                IsDrillingInstallment = request.IsDrillingInstallment,
                DrillingFirstContribution = request.DrillingFirstContribution,
                DrillingFirstPayment = request.DrillingFirstPayment,
                DrillingFirstPaymentDueDate = request.DrillingFirstPaymentDueDate,
                DrillingSecondPayment = request.DrillingSecondPayment,
                DrillingSecondPaymentDueDate = request.DrillingSecondPaymentDueDate,
                DrillingThirdPayment = request.DrillingThirdPayment,
                DrillingThirdPaymentDueDate = request.DrillingThirdPaymentDueDate,
                DrillingFourthPayment = request.DrillingFourthPayment,
                DrillingFourthPaymentDueDate = request.DrillingFourthPaymentDueDate,
                IsArrangementInstallment = request.IsArrangementInstallment,
                ArrangementFirstContribution = request.ArrangementFirstContribution,
                ArrangementFirstPayment = request.ArrangementFirstPayment,
                ArrangementFirstPaymentDueDate = request.ArrangementFirstPaymentDueDate,
                ArrangementSecondPayment = request.ArrangementSecondPayment,
                ArrangementSecondPaymentDueDate = request.ArrangementSecondPaymentDueDate,
                ArrangementThirdPayment = request.ArrangementThirdPayment,
                ArrangementThirdPaymentDueDate = request.ArrangementThirdPaymentDueDate,
                ArrangementFourthPayment = request.ArrangementFourthPayment,
                ArrangementFourthPaymentDueDate = request.ArrangementFourthPaymentDueDate,
                TotalDrillingAmount = request.TotalDrillingAmount,
                TotalArrangementAmount = request.TotalArrangementAmount,
                InstallmentEripNumber = request.InstallmentEripNumber,
                Info = request.Info,
                Status = request.Status,
                BrigadeStatus = request.BrigadeStatus,
                WorkDate = request.WorkDate,
                ArrangementDate = request.ArrangementDate,
                Contractor = request.Contractor,
                Coordinates = request.Coordinates,
                Sewer = request.Sewer,
                DrillingBrigadeId = request.DrillingBrigadeId,
                ArrangementBrigadeId = request.ArrangementBrigadeId,
                CreatedBy = currentUser.FullName
            };

            var created = await _orderServices.CreateAsync(model);
            await _notificationService.CreateNewOrderNotificationsAsync(created);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, ToDetails(created));
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<OrderDetailsResponse>> Update([FromRoute] int id, [FromBody] OrderUpsertRequest request)
        {
            var existing = await _orderServices.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            if (!await CanAccessOrder(existing))
            {
                return Forbid();
            }

            existing.NameClient = request.NameClient;
            existing.SurnameClient = request.SurnameClient ?? string.Empty;
            existing.Phone = request.Phone;
            existing.Area = request.Area ?? string.Empty;
            existing.District = request.District ?? string.Empty;
            existing.City = request.City ?? string.Empty;
            existing.Diameter = request.Diameter;
            existing.PricePerMeter = request.PricePerMeter;
            existing.Pump = request.Pump;
            existing.MetersCount = request.MetersCount;
            existing.Depth = request.Depth;
            existing.StaticLevel = request.StaticLevel;
            existing.DynamicLevel = request.DynamicLevel;
            existing.Filter = request.Filter;
            existing.PumpModel = request.PumpModel;
            existing.Arrangement = request.Arrangement ?? "Не нужно";
            existing.PumpInstalled = request.PumpInstalled;
            existing.ArrangementDone = request.ArrangementDone;
            existing.IsDrillingInstallment = request.IsDrillingInstallment;
            existing.DrillingFirstContribution = request.DrillingFirstContribution;
            existing.DrillingFirstPayment = request.DrillingFirstPayment;
            existing.DrillingFirstPaymentDueDate = request.DrillingFirstPaymentDueDate;
            existing.DrillingSecondPayment = request.DrillingSecondPayment;
            existing.DrillingSecondPaymentDueDate = request.DrillingSecondPaymentDueDate;
            existing.DrillingThirdPayment = request.DrillingThirdPayment;
            existing.DrillingThirdPaymentDueDate = request.DrillingThirdPaymentDueDate;
            existing.DrillingFourthPayment = request.DrillingFourthPayment;
            existing.DrillingFourthPaymentDueDate = request.DrillingFourthPaymentDueDate;
            existing.IsArrangementInstallment = request.IsArrangementInstallment;
            existing.ArrangementFirstContribution = request.ArrangementFirstContribution;
            existing.ArrangementFirstPayment = request.ArrangementFirstPayment;
            existing.ArrangementFirstPaymentDueDate = request.ArrangementFirstPaymentDueDate;
            existing.ArrangementSecondPayment = request.ArrangementSecondPayment;
            existing.ArrangementSecondPaymentDueDate = request.ArrangementSecondPaymentDueDate;
            existing.ArrangementThirdPayment = request.ArrangementThirdPayment;
            existing.ArrangementThirdPaymentDueDate = request.ArrangementThirdPaymentDueDate;
            existing.ArrangementFourthPayment = request.ArrangementFourthPayment;
            existing.ArrangementFourthPaymentDueDate = request.ArrangementFourthPaymentDueDate;
            existing.TotalDrillingAmount = request.TotalDrillingAmount;
            existing.TotalArrangementAmount = request.TotalArrangementAmount;
            existing.InstallmentEripNumber = request.InstallmentEripNumber;
            existing.Info = request.Info;
            existing.Status = request.Status;
            existing.BrigadeStatus = request.BrigadeStatus;
            existing.WorkDate = request.WorkDate;
            existing.ArrangementDate = request.ArrangementDate;
            existing.Contractor = request.Contractor;
            existing.Coordinates = request.Coordinates;
            existing.Sewer = request.Sewer;
            existing.DrillingBrigadeId = request.DrillingBrigadeId;
            existing.ArrangementBrigadeId = request.ArrangementBrigadeId;

            var updated = await _orderServices.UpdateAsync(existing);
            var editor = (await GetCurrentUserProfile())?.FullName;
            await _notificationService.NotifyOrderUpdatedAsync(updated, editor);
            return Ok(ToDetails(updated));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var existing = await _orderServices.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            if (!await CanAccessOrder(existing))
            {
                return Forbid();
            }

            var actor = (await GetCurrentUserProfile())?.FullName;
            await _notificationService.NotifyOrderDeletedAsync(existing, actor);
            await _orderServices.DeleteAsync(id);
            return NoContent();
        }

        private OrderDetailsResponse ToDetails(OrderModelData o)
        {
            return new OrderDetailsResponse(
                Id: o.Id,
                NameClient: o.NameClient,
                SurnameClient: o.SurnameClient,
                Phone: o.Phone,
                Area: o.Area,
                District: o.District,
                City: o.City,
                Diameter: o.Diameter,
                PricePerMeter: o.PricePerMeter,
                Pump: o.Pump,
                MetersCount: o.MetersCount,
                Depth: o.Depth,
                StaticLevel: o.StaticLevel,
                DynamicLevel: o.DynamicLevel,
                Filter: o.Filter,
                PumpModel: o.PumpModel,
                Arrangement: o.Arrangement,
                PumpInstalled: o.PumpInstalled,
                ArrangementDone: o.ArrangementDone,
                IsDrillingInstallment: o.IsDrillingInstallment,
                DrillingFirstContribution: o.DrillingFirstContribution,
                DrillingFirstPayment: o.DrillingFirstPayment,
                DrillingFirstPaymentDueDate: o.DrillingFirstPaymentDueDate,
                DrillingSecondPayment: o.DrillingSecondPayment,
                DrillingSecondPaymentDueDate: o.DrillingSecondPaymentDueDate,
                DrillingThirdPayment: o.DrillingThirdPayment,
                DrillingThirdPaymentDueDate: o.DrillingThirdPaymentDueDate,
                DrillingFourthPayment: o.DrillingFourthPayment,
                DrillingFourthPaymentDueDate: o.DrillingFourthPaymentDueDate,
                IsArrangementInstallment: o.IsArrangementInstallment,
                ArrangementFirstContribution: o.ArrangementFirstContribution,
                ArrangementFirstPayment: o.ArrangementFirstPayment,
                ArrangementFirstPaymentDueDate: o.ArrangementFirstPaymentDueDate,
                ArrangementSecondPayment: o.ArrangementSecondPayment,
                ArrangementSecondPaymentDueDate: o.ArrangementSecondPaymentDueDate,
                ArrangementThirdPayment: o.ArrangementThirdPayment,
                ArrangementThirdPaymentDueDate: o.ArrangementThirdPaymentDueDate,
                ArrangementFourthPayment: o.ArrangementFourthPayment,
                ArrangementFourthPaymentDueDate: o.ArrangementFourthPaymentDueDate,
                TotalDrillingAmount: o.TotalDrillingAmount,
                TotalArrangementAmount: o.TotalArrangementAmount,
                InstallmentEripNumber: o.InstallmentEripNumber,
                Info: o.Info,
                Status: o.Status,
                BrigadeStatus: o.BrigadeStatus,
                CreatedBy: o.CreatedBy,
                CreationTimeData: o.CreationTimeData,
                WorkDate: o.WorkDate,
                ArrangementDate: o.ArrangementDate,
                Contractor: o.Contractor,
                Coordinates: o.Coordinates,
                Sewer: o.Sewer,
                DrillingBrigadeId: o.DrillingBrigadeId,
                ArrangementBrigadeId: o.ArrangementBrigadeId
            );
        }

        private async Task<UserModelData?> GetCurrentUserProfile()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrWhiteSpace(email))
            {
                return null;
            }

            var allUsers = await _userServices.GetAllAsync();
            return allUsers.FirstOrDefault(u => u.Email == email);
        }

        private async Task<List<OrderModelData>> FilterOrdersForCurrentUser(List<OrderModelData> orders)
        {
            var currentUser = await GetCurrentUserProfile();
            if (currentUser == null || string.IsNullOrWhiteSpace(currentUser.Role))
            {
                return orders;
            }

            if (currentUser.Role != "DrillingMaster" && currentUser.Role != "MountingMaster")
            {
                return orders;
            }

            var brigades = await _brigadeServices.GetAllAsync();
            var userBrigadeIds = brigades
                .Where(b => b.ResponsibleUserId == currentUser.Id)
                .Select(b => b.Id)
                .ToList();

            if (!userBrigadeIds.Any())
            {
                return new List<OrderModelData>();
            }

            if (currentUser.Role == "DrillingMaster")
            {
                return orders
                    .Where(o => o.DrillingBrigadeId.HasValue && userBrigadeIds.Contains(o.DrillingBrigadeId.Value))
                    .ToList();
            }

            return orders
                .Where(o => o.ArrangementBrigadeId.HasValue && userBrigadeIds.Contains(o.ArrangementBrigadeId.Value))
                .ToList();
        }

        private async Task<bool> CanAccessOrder(OrderModelData order)
        {
            var filtered = await FilterOrdersForCurrentUser(new List<OrderModelData> { order });
            return filtered.Any();
        }
    }
}

