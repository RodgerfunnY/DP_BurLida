using DP_BurLida.Data.ModelsData;

namespace DP_BurLida.Models
{
    public class InstallmentRowViewModel
    {
        public int OrderId { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string ClientName { get; set; }
        public decimal TotalRemainingAmount { get; set; }
        public decimal CurrentPaymentAmount { get; set; }
        public DateTime? CurrentPaymentDueDate { get; set; }
        public string? InstallmentEripNumber { get; set; }
        public string? CurrentPaymentSlotKey { get; set; }
        public OrderModelData Order { get; set; }
    }

    public class InstallmentListViewModel
    {
        public List<InstallmentRowViewModel> Items { get; set; } = new();
    }
}

