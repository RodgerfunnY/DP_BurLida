using DP_BurLida.Data.ModelsData;
using Microsoft.AspNetCore.Mvc;

namespace DP_BurLida.Services.Interfaces
{
    public interface IOrderServices
    {
        public IEnumerable<OrderModelData> GetAll();
        public ActionResult<OrderModelData> GetById(int id);
        public ActionResult<OrderModelData> Create(OrderModelData order);
        public ActionResult<OrderModelData> Delete(int id);
        public ActionResult<OrderModelData> Update(OrderModelData order);
    }
}
