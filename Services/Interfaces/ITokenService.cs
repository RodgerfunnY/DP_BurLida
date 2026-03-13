using DP_BurLida.Data.ModelsData;

namespace DP_BurLida.Services.Interfaces
{
    public interface ITokenService
    {
        string CreateAccessToken(UserModelData user);
    }
}

