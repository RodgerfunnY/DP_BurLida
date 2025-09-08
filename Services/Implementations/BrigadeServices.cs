using DP_BurLida.Data;
using DP_BurLida.Data.ModelsData;
using DP_BurLida.Services.CRUDServics;
using DP_BurLida.Services.Interfaces;

namespace DP_BurLida.Services.Implementations
{
    public class BrigadeServices : CrudServices<BrigadeModelData>, IBrigadeServices
    {
        public BrigadeServices(ByrlidaContext context) : base(context)
        {
        }
    
    }
}
