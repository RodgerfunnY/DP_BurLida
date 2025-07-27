using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DP_BurLida.Data.ModelsData
{
    public class UserModelData : IdentityUser
    //создание модели юзера от которой будет потом унаследование каждой модели
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public UserModelData(int id, string name, string surname, string email, string password, string phone)
        {
            Id = id;
            Name = name;
            Surname = surname;
            Email = email;
            Password = password;
            Phone = phone;
        }
    }

}
