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
    }
}