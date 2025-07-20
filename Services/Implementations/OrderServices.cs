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
    }
}