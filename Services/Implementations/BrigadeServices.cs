using DP_BurLida.Data;
using DP_BurLida.Data.ModelsData;
using DP_BurLida.Services.CRUDServics;
using DP_BurLida.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DP_BurLida.Services.Implementations
{
    public class BrigadeServices : CrudServices<BrigadeModelData>, IBrigadeServices
    {
        public BrigadeServices(ByrlidaContext context) : base(context)
        {
        }

        // Переопределяем метод для загрузки связанных данных
        public override async Task<List<BrigadeModelData>> GetAllAsync()
        {
            return await _context.Set<BrigadeModelData>()
                .Include(b => b.ResponsibleUser)
                .ToListAsync();
        }

        // Переопределяем метод GetByIdAsync для загрузки связанных данных
        public override async Task<BrigadeModelData> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException($"Некорекктный ID - {id}");
            }
            return await _context.Set<BrigadeModelData>()
                .Include(b => b.ResponsibleUser)
                .FirstOrDefaultAsync(b => b.Id == id);
        }
    }
}
