using DP_BurLida.Data.ModelsData;

namespace DP_BurLida.Services.Interfaces
{
    public interface IOrderServices
    {
        Task<IEnumerable<OrderModelData>> GetAllOrder();
        Task AddOrder(OrderModelData order);
        Task UpdateOrder(OrderModelData order);
        Task DeleteOrder(int id);
        Task<OrderModelData> GetOrderById(int id);
    }
}