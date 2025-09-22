using System.ComponentModel.DataAnnotations;

namespace DP_BurLida.Data.ModelsData
{
    public class BrigadeModelData
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Название бригады обязательно")]
        [StringLength(100, ErrorMessage = "Название бригады не должно превышать 100 символов")]
        public string NameBrigade { get; set; }
        
        [StringLength(200, ErrorMessage = "Техника не должна превышать 200 символов")]
        public string Technic { get; set; }
        
        [StringLength(500, ErrorMessage = "Информация не должна превышать 500 символов")]
        public string Info { get; set; }
        
        public int? ResponsibleUserId { get; set; }
        
        // Исключаем навигационное свойство из валидации и маппинга
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        [System.ComponentModel.DataAnnotations.ScaffoldColumn(false)]
        public UserModelData ResponsibleUser { get; set; }

        public BrigadeModelData(int id, string nameBrigade, string technic,
                               string info, int? responsibleUserId, UserModelData responsibleUser)
        {
            Id = id;
            NameBrigade = nameBrigade;
            Technic = technic;
            Info = info;
            ResponsibleUserId = responsibleUserId;
            ResponsibleUser = responsibleUser;
        }

        public BrigadeModelData() { }
    }
}
