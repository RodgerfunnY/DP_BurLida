using DP_BurLida.Data;
using DP_BurLida.Data.ModelsData;
using DP_BurLida.Services.CRUDServics;
using DP_BurLida.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DP_BurLida.Services.Implementations
{
    public class UserServices : CrudServices<UserModelData>, IUserServices
    {
        public UserServices(ByrlidaContext context) : base(context)
        {
        }
    }
}
