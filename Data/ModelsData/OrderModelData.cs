using System.ComponentModel.DataAnnotations;

namespace DP_BurLida.Data.ModelsData
{
    //Модель заявки
    public class OrderModelData
    {
        public int Id { get; set; }
        public string NameClient { get; set; }
        public string? SurnameClient { get; set; }
        public string Phone { get; set; }
        public string? Area { get; set; }
        public string? District { get; set; }
        [MaxLength(1000)]
        public string? City { get; set; }
        public int Diameter { get; set; }
        public int PricePerMeter { get; set; }
        public int Pump { get; set; }
        /// <summary>
        /// Количество метров (необязательное поле).
        /// </summary>
        public int? MetersCount { get; set; }
        /// <summary>
        /// Глубина (необязательное поле).
        /// </summary>
        [MaxLength(1000)]
        public string? Depth { get; set; }
        /// <summary>
        /// Статика (необязательное поле).
        /// </summary>
        [MaxLength(1000)]
        public string? StaticLevel { get; set; }
        /// <summary>
        /// Динамика (необязательное поле).
        /// </summary>
        [MaxLength(1000)]
        public string? DynamicLevel { get; set; }
        /// <summary>
        /// Фильтр (необязательное поле).
        /// </summary>
        [MaxLength(1000)]
        public string? Filter { get; set; }
        /// <summary>
        /// Модель насоса (необязательное поле).
        /// </summary>
        public string? PumpModel { get; set; }
        /// <summary>
        /// Тип обустройства, свободный ввод (необязательное поле).
        /// </summary>
        public string? Arrangement { get; set; }
        /// <summary>
        /// Монтировали насос (необязательное поле).
        /// </summary>
        [MaxLength(1000)]
        public string? PumpInstalled { get; set; }
        /// <summary>
        /// Обустройство выполнено (необязательное поле).
        /// </summary>
        [MaxLength(1000)]
        public string? ArrangementDone { get; set; }
        [MaxLength(1000)]
        public string Info { get; set; }
        public string Status { get; set; }
        public DateTime CreationTimeData { get; set; } = DateTime.Now;
        /// <summary>
        /// Дата бурения (ранее Дата работы).
        /// </summary>
        public DateTime? WorkDate { get; set; }
        /// <summary>
        /// Дата обустройства (необязательное поле).
        /// </summary>
        public DateTime? ArrangementDate { get; set; }

        /// <summary>
        /// Кому отдать подряд (Саша, Олег, Александр) – необязательное поле.
        /// </summary>
        public string? Contractor { get; set; }

        /// <summary>
        /// Координаты объекта (для отображения на карте).
        /// Формат можно использовать как \"широта,долгота\".
        /// </summary>
        [MaxLength(1000)]
        public string? Coordinates { get; set; }

        /// <summary>
        /// Описание канализации (необязательное текстовое поле).
        /// </summary>
        [MaxLength(1000)]
        public string? Sewer { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public string? Address { get; set; }

        public int? DrillingBrigadeId { get; set; }
        public int? ArrangementBrigadeId { get; set; }
        
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        [System.ComponentModel.DataAnnotations.ScaffoldColumn(false)]
        public BrigadeModelData DrillingBrigade { get; set; }
        
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        [System.ComponentModel.DataAnnotations.ScaffoldColumn(false)]
        public BrigadeModelData ArrangementBrigade { get; set; }

        public OrderModelData() { }
    }
}

