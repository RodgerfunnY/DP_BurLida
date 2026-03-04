using DP_BurLida.Data.ModelsData;

namespace DP_BurLida.Models
{
    public class StatisticsViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public List<ManagerStatisticsItem> Managers { get; set; } = new List<ManagerStatisticsItem>();
        public List<DrillingBrigadeStatisticsItem> DrillingBrigades { get; set; } = new List<DrillingBrigadeStatisticsItem>();
        public List<MountingMasterStatisticsItem> MountingMasters { get; set; } = new List<MountingMasterStatisticsItem>();
    }

    public class ManagerStatisticsItem
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int TotalOrders { get; set; }
    }

    public class DrillingBrigadeStatisticsItem
    {
        public int BrigadeId { get; set; }
        public string BrigadeName { get; set; }
        public string? ResponsibleMasterName { get; set; }
        public int TotalMeters { get; set; }
    }

    public class MountingMasterStatisticsItem
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int TotalCompletedObjects { get; set; }
    }
}

