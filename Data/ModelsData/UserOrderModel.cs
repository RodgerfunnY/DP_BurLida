using Microsoft.AspNetCore.Identity;

namespace DP_BurLida.Data.ModelsData
{
    public class UserModelData
    //создание модели юзера от которой будет потом унаследование каждой модели
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public UserModelData(int id, string name, string surname, string email, string phone)
        {
            Id = id;
            Name = name;
            Surname = surname;
            Email = email;
            Phone = phone;
        }
    }
}
