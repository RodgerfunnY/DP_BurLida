using DP_BurLida.Data.ModelsData;
using DP_BurLida.Services.InterfacesServics;
using Microsoft.AspNetCore.Mvc;

namespace DP_BurLida.Services.Interfaces
{
    public interface IUserServices : ICrudServices <UserModelData>
    {

    }
}
