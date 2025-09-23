using System.ComponentModel.DataAnnotations;

namespace DP_BurLida.Data.ModelsData
{
    //Модель заявки
    public class OrderModelData
    {
        public int Id { get; set; }
        public string NameClient { get; set; }
        public string SurnameClient { get; set; }
        public string Phone { get; set; }
        public string Area { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public int Diameter { get; set; }
        public int PricePerMeter { get; set; }
        public int Pump { get; set; }
        public string Arrangement { get; set; }
        public string Info { get; set; }
        public string Status { get; set; }
        public DateTime CreationTimeData { get; set; } = DateTime.Now;
        public DateTime? WorkDate { get; set; }
        
        public int? DrillingBrigadeId { get; set; }
        public int? ArrangementBrigadeId { get; set; }
        
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        [System.ComponentModel.DataAnnotations.ScaffoldColumn(false)]
        public BrigadeModelData DrillingBrigade { get; set; }
        
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        [System.ComponentModel.DataAnnotations.ScaffoldColumn(false)]
        public BrigadeModelData ArrangementBrigade { get; set; }

        public OrderModelData() { }
        public OrderModelData(int id, string nameClient, string surnameClient, string phone, string area, string district,
            string city, int diameter, int pricePerMeter, int pump, string arrangement, string info, string status, DateTime creationTimeData, DateTime? workDate = null, 
            int? drillingBrigadeId = null, int? arrangementBrigadeId = null)
        {
            Id = id;
            NameClient = nameClient;
            SurnameClient = surnameClient;
            Phone = phone;
            Area = area;
            District = district;
            City = city;
            Diameter = diameter;
            PricePerMeter = pricePerMeter;
            Pump = pump;
            Arrangement = arrangement;
            Info = info;
            Status = status;
            CreationTimeData = creationTimeData;
            WorkDate = workDate;
            DrillingBrigadeId = drillingBrigadeId;
            ArrangementBrigadeId = arrangementBrigadeId;
        }
    }
}

