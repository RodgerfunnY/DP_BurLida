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
        public IEnumerable<OrderModelData> GetAll() => _context.OrderModelData.ToList();

        public ActionResult<OrderModelData> GetById(int id)
        {
            var orderPoisk = _context.OrderModelData.FirstOrDefault(o => o.Id == id);
            return orderPoisk;
        }

        public ActionResult<OrderModelData> Create([FromBody] OrderModelData order)
        {
            _context.OrderModelData.Add(order);
            _context.SaveChanges();
            return order;
        }

        public ActionResult<OrderModelData> Delete(int id)
        {
            var orderDelete = _context.OrderModelData.FirstOrDefault(o => o.Id == id);
            if (orderDelete != null)
            {
                _context.OrderModelData.Remove(orderDelete);
                _context.SaveChanges();
                return orderDelete;
            }
            return orderDelete;


        }
        public ActionResult<OrderModelData> Update(OrderModelData order)
        {
            var orderUpdate = _context.OrderModelData.FirstOrDefault(o => o.Id == order.Id);
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

                _context.SaveChanges();
                return orderUpdate;
            }
            return orderUpdate;
        }
    }
}