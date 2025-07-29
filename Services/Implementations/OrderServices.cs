using DP_BurLida.Data;
using DP_BurLida.Data.ModelsData;
using DP_BurLida.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace DP_BurLida.Services.Implementations
{
    public class OrderServices : IOrderServices
    {
        private readonly ByrlidaContext _context;

        public OrderServices(ByrlidaContext context)
        {
            _context = context;
        }
        public async Task<List<OrderModelData>> GetAllAsync() => await _context.OrderModelData.ToListAsync();

        public async Task<OrderModelData> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException($"Некорекктный ID - {id}");
            }
            return await _context.OrderModelData.FirstOrDefaultAsync(o => o.Id == id); 
        }

        public async Task<OrderModelData> CreateAsync([FromBody] OrderModelData order)
        {
            await _context.OrderModelData.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<OrderModelData> DeleteAsync(int id)
        {
            var orderDelete = await _context.OrderModelData.FirstOrDefaultAsync(o => o.Id == id);
            if (orderDelete != null)
            {
                _context.OrderModelData.Remove(orderDelete);
                await _context.SaveChangesAsync();
                return orderDelete;
            }
            return orderDelete;


        }
        public async Task<OrderModelData> UpdateAsync(OrderModelData order)
        {
            var orderUpdate = await _context.OrderModelData.FirstOrDefaultAsync(o => o.Id == order.Id);
            if (orderUpdate != null)
            {
                // Копируем значения из order в orderUpdate
                orderUpdate.NameClient = order.NameClient;
                orderUpdate.SurnameClient = order.SurnameClient;
                orderUpdate.Phone = order.Phone;
                orderUpdate.Area = order.Area;
                orderUpdate.District = order.District;
                orderUpdate.City = order.City;
                orderUpdate.Diameter = order.Diameter;
                orderUpdate.PricePerMeter = order.PricePerMeter;
                orderUpdate.Pump = order.Pump;
                orderUpdate.Arrangement = order.Arrangement;
                orderUpdate.Info = order.Info;
                orderUpdate.CreationTimeData = order.CreationTimeData;

                await _context.SaveChangesAsync();
                return orderUpdate;
            }
            return orderUpdate;
        }
    }
}