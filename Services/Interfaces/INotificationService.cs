using DP_BurLida.Data.ModelsData;

namespace DP_BurLida.Services.Interfaces
{
    public interface INotificationService
    {
        Task CreateNewOrderNotificationsAsync(OrderModelData order);
    }
}

