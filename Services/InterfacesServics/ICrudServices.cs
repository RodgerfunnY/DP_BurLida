using DP_BurLida.Data.ModelsData;

namespace DP_BurLida.Services.InterfacesServics
{
    public interface ICrudServices <TModel> where TModel : class
    {
        public Task<List<TModel>> GetAllAsync();
        public Task<TModel> GetByIdAsync(int id);
        public Task<TModel> CreateAsync(TModel model);
        public Task<TModel> DeleteAsync(int id);
        public Task<TModel> UpdateAsync(TModel model);
    }
}
