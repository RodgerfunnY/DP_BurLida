using DP_BurLida.Data;
using DP_BurLida.Data.ModelsData;
using DP_BurLida.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace DP_BurLida.Services.Implementations
{
    public class OrderServices : IOrderServices
    {
        private readonly ByrlidaContext _context;

        public OrderServices(ByrlidaContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<OrderModelData>> GetAllOrder()
        {
            return await _context.OrderModelData.ToListAsync();
        }
        public async Task AddOrder(OrderModelData order)
        {
            _context.OrderModelData.Add(order);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateOrder(OrderModelData order)
        {
            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteOrder(int id)
        {
            var order = await _context.OrderModelData.FindAsync(id);
            if (order != null)
            {
                _context.OrderModelData.Remove(order);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<OrderModelData> GetOrderById(int id)
        {
            return await _context.OrderModelData.FindAsync(id);
        }
    }
}