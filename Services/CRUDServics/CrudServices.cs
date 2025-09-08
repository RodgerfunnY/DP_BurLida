using DP_BurLida.Data;
using DP_BurLida.Data.ModelsData;
using DP_BurLida.Services.InterfacesServics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DP_BurLida.Services.CRUDServics
{
    public class CrudServices<TModel> : ICrudServices<TModel> where TModel : class
    {
        private readonly ByrlidaContext _context;

        public CrudServices(ByrlidaContext context)
        {
            _context = context;
        }
        public async Task<List<TModel>> GetAllAsync() => await _context.Set<TModel>().ToListAsync();

        public async Task<TModel> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException($"Некорекктный ID - {id}");
            }
            return await _context.Set<TModel>().FindAsync(id);
        }

        public async Task<TModel> CreateAsync(TModel model)
        {
            await _context.Set<TModel>().AddAsync(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var orderDelete = await _context.Set<TModel>().FindAsync(id);
            if (orderDelete != null)
            {
                _context.Set<TModel>().Remove(orderDelete);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<TModel> UpdateAsync(TModel model)
        {
            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return model;
        }
    }
}
