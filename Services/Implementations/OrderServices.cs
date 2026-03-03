using DP_BurLida.Data;
using DP_BurLida.Data.ModelsData;
using DP_BurLida.Services.CRUDServics;
using DP_BurLida.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace DP_BurLida.Services.Implementations
{
    public class OrderServices : CrudServices<OrderModelData>, IOrderServices
    {
        public OrderServices(ByrlidaContext context) : base(context)
        {
        }

        public override async Task<List<OrderModelData>> GetAllAsync()
        {
            return await _context.Set<OrderModelData>()
                .Include(o => o.DrillingBrigade)
                .Include(o => o.ArrangementBrigade)
                .ToListAsync();
        }

        public override async Task<OrderModelData> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException($"Некорректный ID - {id}");
            var order = await _context.Set<OrderModelData>()
                .Include(o => o.DrillingBrigade)
                .Include(o => o.ArrangementBrigade)
                .FirstOrDefaultAsync(o => o.Id == id);
            if (order == null)
            {
                
                throw new InvalidOperationException($"Заявка с ID={id} не найдена.");
            }
            return order;
        }

        public async Task<List<OrderModelData>> SearchAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetAllAsync();
            }

            var allOrders = await _context.Set<OrderModelData>()
                .Include(o => o.DrillingBrigade)
                .Include(o => o.ArrangementBrigade)
                .ToListAsync();

            var searchLower = searchTerm.ToLower();

            return allOrders
                .Where(order =>
                    (!string.IsNullOrEmpty(order.NameClient) && order.NameClient.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(order.Phone) && order.Phone.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(order.City) && order.City.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(order.Arrangement) && order.Arrangement.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(order.Info) && order.Info.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(order.Contractor) && order.Contractor.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(order.Coordinates) && order.Coordinates.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(order.Sewer) && order.Sewer.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(order.Depth) && order.Depth.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(order.StaticLevel) && order.StaticLevel.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(order.DynamicLevel) && order.DynamicLevel.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(order.Filter) && order.Filter.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(order.PumpInstalled) && order.PumpInstalled.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(order.ArrangementDone) && order.ArrangementDone.ToLower().Contains(searchLower))
                )
                .ToList();
        }
    }
}