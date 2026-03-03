namespace DP_BurLida.Data.ModelsData
{
    public class UserModelData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        
        /// <summary>
        /// Роль пользователя в системе:
        /// Admin, Director, Manager, DrillingMaster, MountingMaster, Pending.
        /// </summary>
        public string Role { get; set; } = "Pending";

        /// <summary>
        /// Подтвержден ли сотрудник администратором или директором.
        /// Только подтвержденные пользователи могут работать в системе.
        /// </summary>
        public bool IsApproved { get; set; } = false;
        
        public string FullName => $"{Name} {Surname}".Trim();

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
