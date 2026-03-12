using System.ComponentModel.DataAnnotations;

namespace DP_BurLida.Data.ModelsData
{
    public class NotificationModelData
    {
        public int Id { get; set; }

        public int? OrderId { get; set; }

        [MaxLength(500)]
        public string Message { get; set; } = string.Empty;

        [MaxLength(255)]
        public string RecipientEmail { get; set; } = string.Empty;

        public bool IsRead { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}

