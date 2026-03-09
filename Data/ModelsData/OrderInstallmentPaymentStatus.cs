using System.ComponentModel.DataAnnotations;

namespace DP_BurLida.Data.ModelsData
{
    /// <summary>
    /// Сохраняет факт оплаты конкретного платежа рассрочки по заявке.
    /// Сумма и дата берутся из OrderModelData, здесь только статус.
    /// </summary>
    public class OrderInstallmentPaymentStatus
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        /// <summary>
        /// Ключ слота платежа: Drilling1..4, Arrangement1..4.
        /// </summary>
        [MaxLength(50)]
        public string SlotKey { get; set; } = string.Empty;

        public bool IsPaid { get; set; }

        public DateTime? PaidAt { get; set; }
    }
}

