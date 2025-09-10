using DP_BurLida.Data;
using DP_BurLida.Data.ModelsData;
using DP_BurLida.Services.CRUDServics;
using DP_BurLida.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace DP_BurLida.Services.Implementations
{
    public class OrderServices : CrudServices<OrderModelData>, IOrderServices
    {
        public OrderServices(ByrlidaContext context) : base(context)
        {
        }

        public async Task<List<OrderModelData>> SearchAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetAllAsync();
            }

            var allOrders = await _context.OrderModelData.ToListAsync();
            var searchLower = searchTerm.ToLower();
            var searchResults = new List<OrderModelData>();


            foreach (var order in allOrders)
            {
                bool found = false;
                
                if (order.NameClient != null && order.NameClient.ToLower().Contains(searchLower))
                {
                    found = true;
                }
                else if (order.SurnameClient != null && order.SurnameClient.ToLower().Contains(searchLower))
                {
                    found = true;
                }
                else if (order.Phone != null && order.Phone.ToLower().Contains(searchLower))
                {
                    found = true;
                }
                else if (order.Area != null && order.Area.ToLower().Contains(searchLower))
                {
                    found = true;
                }
                else if (order.District != null && order.District.ToLower().Contains(searchLower))
                {
                    found = true;
                }
                else if (order.City != null && order.City.ToLower().Contains(searchLower))
                {
                    found = true;
                }
                else if (order.Arrangement != null && order.Arrangement.ToLower().Contains(searchLower))
                {
                    found = true;
                }
                else if (order.Info != null && order.Info.ToLower().Contains(searchLower))
                {
                    found = true;
                }
                

                if (found)
                {
                    searchResults.Add(order);
                }
            }

            return searchResults;
        }
    }
}