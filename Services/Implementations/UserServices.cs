using DP_BurLida.Data;
using DP_BurLida.Data.ModelsData;
using DP_BurLida.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DP_BurLida.Services.Implementations
{
    public class UserServices : IUserServices
    {
        private readonly ByrlidaContext _context;

        public UserServices(ByrlidaContext context)
        {
            _context = context;
        }
        public async Task<List<UserModelData>> GetAllAsync() => await _context.UserModelData.ToListAsync();

        public async Task<UserModelData> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException($"Некорекктный ID - {id}");
            }
            return await _context.UserModelData.FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<UserModelData> CreateAsync([FromBody] UserModelData user)
        {
            await _context.UserModelData.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<UserModelData> DeleteAsync(int id)
        {
            var userDelete = await _context.UserModelData.FirstOrDefaultAsync(o => o.Id == id);
            if (userDelete != null)
            {
                _context.UserModelData.Remove(userDelete);
                await _context.SaveChangesAsync();
                return userDelete;
            }
            return userDelete;
        }

        public async Task<UserModelData> UpdateAsync(UserModelData user)
        {
            var userUpdate = await _context.UserModelData.FirstOrDefaultAsync(o => o.Id == user.Id);
            if (userUpdate != null)
            {
                // Копируем значения из order в orderUpdate
                userUpdate.Name = user.Name;
                userUpdate.Surname = user.Surname;
                userUpdate.Phone = user.Phone;
                userUpdate.Email = user.Email;
                await _context.SaveChangesAsync();
                return userUpdate;
            }
            return userUpdate;
        }
    }
}
