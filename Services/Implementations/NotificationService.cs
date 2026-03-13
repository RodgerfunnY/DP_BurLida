using DP_BurLida.Data;
using DP_BurLida.Data.ModelsData;
using DP_BurLida.Services.Interfaces;

namespace DP_BurLida.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly ByrlidaContext _context;
        private readonly IUserServices _userServices;
        private readonly IPushNotificationService _push;

        public NotificationService(ByrlidaContext context, IUserServices userServices, IPushNotificationService push)
        {
            _context = context;
            _userServices = userServices;
            _push = push;
        }

        public async Task CreateNewOrderNotificationsAsync(OrderModelData order)
        {
            try
            {
                var allUsers = await _userServices.GetAllAsync();
                var recipients = allUsers
                    .Where(u => u.IsApproved && (u.Role == "Admin" || u.Role == "Director" || u.Role == "Manager"))
                    .Where(u => !string.IsNullOrWhiteSpace(u.Email))
                    .Select(u => u.Email.Trim())
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();

                if (recipients.Count == 0)
                {
                    return;
                }

                var message = $"Создана новая заявка: {order.City} — {order.NameClient}, телефон {order.Phone}";
                var list = recipients.Select(email => new NotificationModelData
                {
                    OrderId = order.Id,
                    Message = message,
                    RecipientEmail = email,
                    IsRead = false,
                    CreatedAt = DateTime.Now
                }).ToList();

                _context.NotificationModelData.AddRange(list);
                await _context.SaveChangesAsync();

                await _push.SendToUsersAsync(
                    userEmails: recipients,
                    title: "Новая заявка",
                    body: message,
                    data: new Dictionary<string, string>
                    {
                        ["type"] = "new_order",
                        ["orderId"] = order.Id.ToString()
                    }
                );
            }
            catch
            {
                // уведомления/пуши не должны ломать создание заявки
            }
        }
    }
}

