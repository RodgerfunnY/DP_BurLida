using DP_BurLida.Data;
using DP_BurLida.Data.ModelsData;
using DP_BurLida.Services.Interfaces;

namespace DP_BurLida.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private const int MaxMessageLength = 500;

        private readonly ByrlidaContext _context;
        private readonly IUserServices _userServices;
        private readonly IPushNotificationService _push;

        public NotificationService(ByrlidaContext context, IUserServices userServices, IPushNotificationService push)
        {
            _context = context;
            _userServices = userServices;
            _push = push;
        }

        public Task CreateNewOrderNotificationsAsync(OrderModelData order)
        {
            var msg = $"Создана новая заявка: {OrderLabel(order)}, тел. {order.Phone}";
            return BroadcastAsync(
                message: msg,
                orderId: order.Id,
                pushTitle: "Новая заявка",
                pushBody: Clamp(msg, 200),
                pushData: new Dictionary<string, string>
                {
                    ["type"] = "new_order",
                    ["orderId"] = order.Id.ToString()
                });
        }

        public Task NotifyOrderUpdatedAsync(OrderModelData order, string? actorDisplayName)
        {
            var msg = $"Заявка изменена: {OrderLabel(order)}. Статус: {order.Status}. Кем: {actorDisplayName ?? "—"}";
            return BroadcastAsync(
                message: msg,
                orderId: order.Id,
                pushTitle: "Заявка изменена",
                pushBody: Clamp(msg, 200),
                pushData: new Dictionary<string, string>
                {
                    ["type"] = "order_updated",
                    ["orderId"] = order.Id.ToString()
                });
        }

        public Task NotifyOrderDeletedAsync(OrderModelData orderSnapshot, string? actorDisplayName)
        {
            var msg = $"Заявка удалена: {OrderLabel(orderSnapshot)}. Кем: {actorDisplayName ?? "—"}";
            return BroadcastAsync(
                message: msg,
                orderId: null,
                pushTitle: "Заявка удалена",
                pushBody: Clamp(msg, 200),
                pushData: new Dictionary<string, string> { ["type"] = "order_deleted" });
        }

        public Task NotifyOrderCommentAddedAsync(int orderId, string orderAddress, string? authorDisplayName, string commentText)
        {
            var preview = Clamp(commentText.Trim(), 180);
            var msg = $"Комментарий к заявке {orderAddress}: {preview} — {authorDisplayName ?? "—"}";
            return BroadcastAsync(
                message: msg,
                orderId: orderId,
                pushTitle: "Комментарий к заявке",
                pushBody: Clamp(msg, 200),
                pushData: new Dictionary<string, string>
                {
                    ["type"] = "order_comment",
                    ["orderId"] = orderId.ToString()
                });
        }

        public Task NotifyOrderCommentToggleDoneAsync(int orderId, string orderAddress, string? actorDisplayName, bool isDone)
        {
            var msg = isDone
                ? $"В заявке {orderAddress} отмечено выполнение пункта комментария. Кем: {actorDisplayName ?? "—"}"
                : $"В заявке {orderAddress} снята отметка выполнения. Кем: {actorDisplayName ?? "—"}";
            return BroadcastAsync(
                message: msg,
                orderId: orderId,
                pushTitle: "Комментарий к заявке",
                pushBody: Clamp(msg, 200),
                pushData: new Dictionary<string, string>
                {
                    ["type"] = "order_comment_done",
                    ["orderId"] = orderId.ToString()
                });
        }

        public Task NotifyOrderScheduleDateChangedAsync(OrderModelData order, DateTime newDate, bool isArrangementDate, string? actorDisplayName)
        {
            var kind = isArrangementDate ? "обустройства" : "бурения";
            var msg = $"Назначена дата {kind} {newDate:dd.MM.yyyy} для заявки {OrderLabel(order)}. Кем: {actorDisplayName ?? "—"}";
            return BroadcastAsync(
                message: msg,
                orderId: order.Id,
                pushTitle: "Дата в графике",
                pushBody: Clamp(msg, 200),
                pushData: new Dictionary<string, string>
                {
                    ["type"] = "order_schedule",
                    ["orderId"] = order.Id.ToString()
                });
        }

        private async Task<List<string>> GetRecipientEmailsAsync()
        {
            var allUsers = await _userServices.GetAllAsync();
            return allUsers
                .Where(u => u.IsApproved && !string.IsNullOrWhiteSpace(u.Email))
                .Select(u => u.Email.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        private async Task BroadcastAsync(
            string message,
            int? orderId,
            string pushTitle,
            string pushBody,
            Dictionary<string, string>? pushData)
        {
            try
            {
                var recipients = await GetRecipientEmailsAsync();
                if (recipients.Count == 0)
                {
                    return;
                }

                message = Clamp(message, MaxMessageLength);
                var list = recipients.Select(email => new NotificationModelData
                {
                    OrderId = orderId,
                    Message = message,
                    RecipientEmail = email,
                    IsRead = false,
                    CreatedAt = DateTime.Now
                }).ToList();

                _context.NotificationModelData.AddRange(list);
                await _context.SaveChangesAsync();

                await _push.SendToUsersAsync(recipients, pushTitle, pushBody, pushData ?? new Dictionary<string, string>());
            }
            catch
            {
                // уведомления/пуши не должны ломать основной сценарий
            }
        }

        private static string OrderLabel(OrderModelData order)
        {
            var city = string.IsNullOrWhiteSpace(order.City) ? "без адреса" : order.City.Trim();
            var client = string.IsNullOrWhiteSpace(order.NameClient) ? "клиент" : order.NameClient.Trim();
            return $"{city} — {client}";
        }

        private static string Clamp(string? text, int maxLen)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            text = text.Trim();
            return text.Length <= maxLen ? text : text[..maxLen] + "…";
        }
    }
}
