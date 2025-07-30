using Microsoft.AspNetCore.Identity;

namespace DP_BurLida.Data.ModelsData
{
    public class UserModelData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public UserModelData() { }
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
