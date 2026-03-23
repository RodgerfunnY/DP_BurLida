using DP_BurLida.Data.ModelsData;

namespace DP_BurLida.Services.Interfaces
{
    public interface INotificationService
    {
        Task CreateNewOrderNotificationsAsync(OrderModelData order);

        Task NotifyOrderUpdatedAsync(OrderModelData order, string? actorDisplayName);

        Task NotifyOrderDeletedAsync(OrderModelData orderSnapshot, string? actorDisplayName);

        Task NotifyOrderCommentAddedAsync(int orderId, string orderAddress, string? authorDisplayName, string commentText);

        Task NotifyOrderCommentToggleDoneAsync(int orderId, string orderAddress, string? actorDisplayName, bool isDone);

        Task NotifyOrderScheduleDateChangedAsync(OrderModelData order, DateTime newDate, bool isArrangementDate, string? actorDisplayName);
    }
}
